using App.Application.Contracts.Infrastructure.UnitOfWork;
using App.Application.Extensions;
using App.Application.UseCases.Contracts;
using App.Common.Bases;
using App.Common.Models.User.Dtos;
using App.Common.Models.User.Request;
using App.Common.Models.User.Result;
using App.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace App.Application.UseCases
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<bool>> ChangePasswordAsync(ChangePasswordRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var user = await _unitOfWork.UserRepository.FindAsync(x => x.Email == request.Email);

                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Message = "Thông tin người dùng không tồn tại!",
                        Success = false,
                        StatusCode = 404,
                    };
                }

                var checkCurrentPassword = PasswordExtension.VerifyPassword(request.CurrentPassword, user.Password, user.Salt);
                if (!checkCurrentPassword)
                {
                    return new BaseResponse<bool>()
                    {
                        Message = "Mật khẩu không chính xác!",
                        Success = false,
                        StatusCode = 400,
                    };
                }

                if(!(request.NewPassword == request.ConfirmPassword))
                {
                    return new BaseResponse<bool>()
                    {
                        Message = "Mật khẩu mới không khớp!",
                        Success = false,
                        StatusCode = 400,
                    };
                }

                var salt = PasswordExtension.GenerateSalt();
                user.Salt = salt;
                user.Password = PasswordExtension.HashPassword(request.NewPassword, salt);

                await _unitOfWork.SaveChangesAsync();

                return new BaseResponse<bool>()
                {
                    Message = "Đổi mật khẩu thành công!",
                    Success = true,
                    StatusCode = 202,
                };
            }
            catch (Exception)
            {
                await _unitOfWork.CancelAsync();
                return new BaseResponse<bool>()
                {
                    Message = "Mật khẩu mới không khớp!",
                    Success = false,
                    StatusCode = 400,
                };
                throw;
            }
        }

        public async Task<BaseResponse<UserViewModel>> CreateUserAsync(CreateUserRequest request)
        {
            try
            {
                var isExist = await _unitOfWork.UserRepository.IsExistAsync(x => x.UserName == request.UserName || x.Email == request.Email);

                if (isExist)
                {
                    return new BaseResponse<UserViewModel>()
                    {
                        Message = "Thông tin người dùng đã tồn tại!",
                        Success = false,
                        StatusCode = 400,
                    };
                }

                string salt = PasswordExtension.GenerateSalt();

                var user = new User
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    FullName = request.UserName,
                    Password = PasswordExtension.HashPassword(request.Password, salt),
                    Salt = salt
                };

                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponse<UserViewModel>()
                {
                    Message = "Tạo người dùng thành công",
                    Success = true,
                    StatusCode = 201,
                    Data = new UserViewModel()
                    {
                        UserName = user.UserName,
                        FullName= user.UserName,
                        Id = user.Id,
                        Email = user.Email,
                    }
                };
            }
            catch (Exception)
            {
                await _unitOfWork.CancelAsync();
                return new BaseResponse<UserViewModel>()
                {
                    Message = "Đăng ký người dùng không thành công!",
                    Success = false,
                    StatusCode = 400,
                };
            }
        }

        public Task<BaseResponse<bool>> DeleteAsync(string UserId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<IEnumerable<UserViewModel>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<UserViewModel>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<LoginResult>> LoginAsync(LoginRequest request, IConfiguration configuration)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.FindAsync(x => x.UserName == request.UserName);

                if (user == null)
                {
                    return new BaseResponse<LoginResult>()
                    {
                        Message = "Thông tin người dùng không tồn tại!",
                        Success = false,
                        StatusCode = 404,
                    };
                }

                var checkPassword = PasswordExtension.VerifyPassword(request.Password, user.Password, user.Salt);

                if (!checkPassword)
                {
                    return new BaseResponse<LoginResult>()
                    {
                        Message = "Mật khẩu không chính xác!",
                        Success = false,
                        StatusCode = 400,
                    };
                }

                // Tạo danh sách claims cho token
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Sid, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.FullName),
                    };

                // Lấy thông tin JWT từ cấu hình
                var issuer = configuration["JwtSettings:Issuer"];
                var audience = configuration["JwtSettings:Audience"];
                var key = configuration["JwtSettings:SecretKey"];
                var expireIn = configuration["JwtSettings:ExpireIn"];

                var accessToken = TokenExtensions.GenerateAccessToken(claims, issuer, audience, key, expireIn);

                if (accessToken == null)
                {
                    return new BaseResponse<LoginResult>()
                    {
                        Message = "Đăng nhập thất bại!",
                        Success = false,
                        StatusCode = 400,
                    };
                }

                return new BaseResponse<LoginResult>()
                {
                    Message = "Đăng nhập thành công",
                    Success = true,
                    StatusCode = 200,
                    Data = new LoginResult()
                    {
                        AccessToken = accessToken,
                        ExpireIn = expireIn,
                    }
                };

            }
            catch (Exception)
            {
                return new BaseResponse<LoginResult>()
                {
                    Message = "Đăng nhập thất bại!",
                    Success = false,
                    StatusCode = 400,
                };
            }
        }
    }
}
