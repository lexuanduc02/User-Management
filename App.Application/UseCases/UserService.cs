using App.Application.Contracts.Infrastructure.UnitOfWork;
using App.Application.Extensions;
using App.Application.UseCases.Contracts;
using App.Common.Bases;
using App.Common.Models.Common;
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

        public async Task<BaseResponse<Guid>> ChangePasswordAsync(ChangePasswordRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var user = await _unitOfWork.UserRepository.FindAsync(x => x.Email == request.Email);

                if (user == null)
                    return BaseResponse<Guid>.NotFound("Không tìm thấy thông tin!");

                var checkCurrentPassword = PasswordExtension.VerifyPassword(request.CurrentPassword, user.Password, user.Salt);
                if (!checkCurrentPassword)
                    return BaseResponse<Guid>.BadRequest("Mật khẩu không chính xác!");

                if (!(request.NewPassword == request.ConfirmPassword))
                    return BaseResponse<Guid>.BadRequest("Mật khẩu không khớp!");

                var salt = PasswordExtension.GenerateSalt();
                user.Salt = salt;
                user.Password = PasswordExtension.HashPassword(request.NewPassword, salt);

                await _unitOfWork.SaveChangesAsync();

                return BaseResponse<Guid>.Success(Guid.Empty, "Đổi mật khẩu thành công!");
            }
            catch (Exception)
            {
                await _unitOfWork.CancelAsync();
                return BaseResponse<Guid>.BadRequest("Đổi mật khẩu thất bại!");
            }
        }

        public async Task<BaseResponse<UserViewModel>> CreateUserAsync(CreateUserRequest request)
        {
            try
            {
                var isExist = await _unitOfWork.UserRepository.IsExistAsync(x => x.UserName == request.UserName || x.Email == request.Email);

                if (isExist)
                    return BaseResponse<UserViewModel>.Conflict("Thông tin đã được đăng ký!");

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

                var data = new UserViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.UserName,
                };

                return BaseResponse<UserViewModel>.Success(data, "Thêm mới thành công!");
            }
            catch (Exception)
            {
                await _unitOfWork.CancelAsync();
                return BaseResponse<UserViewModel>.BadRequest("Thêm mới thất bại!");
            }
        }

        public async Task<BaseResponse<Guid>> DeleteAsync(string userId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var user = await _unitOfWork.UserRepository.GetByIdAsync(Guid.Parse(userId));

                if(user == null)
                    return BaseResponse<Guid>.NotFound("Không tìm thấy thông tin!");

                _unitOfWork.UserRepository.Delete(user);
                await _unitOfWork.SaveChangesAsync();

                return BaseResponse<Guid>.Deleted("Xóa thành công!");
            }
            catch (Exception)
            {
                await _unitOfWork.CancelAsync();
                return BaseResponse<Guid>.BadRequest("Xóa không thành công!");
            }
        }

        public async Task<BaseResponse<PagedList<UserViewModel>>> GetAllAsync(GetListRequest request)
        {
            try
            {
                var query = await _unitOfWork.UserRepository.GetAllAsync();

                if(query == null)
                    return BaseResponse<PagedList<UserViewModel>>.BadRequest("Lấy danh sách không thành công!");

                if (request.Keyword != null)
                {
                    query = query.Where(x => x.FullName.Contains(request.Keyword));
                }

                var data = query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                    .Select(x => new UserViewModel()
                    {
                        Email = x.Email,
                        UserName = x.UserName,
                        FullName = x.UserName,
                        Id = x.Id,
                    }).ToList();

                var totalCount = query.Count();
                var totalPage = (int)Math.Ceiling((double)totalCount / request.PageSize);
                var result = new PagedList<UserViewModel>()
                {
                    CurrentPage = request.PageIndex,
                    TotalPage = totalPage,
                    TotalCount = totalCount,
                    Items = data,
                    PageSize = request.PageSize,
                };

                return BaseResponse<PagedList<UserViewModel>>.Success(result, "Lấy danh sách thành công!");
            }
            catch (Exception)
            {
                return BaseResponse<PagedList<UserViewModel>>.BadRequest("Lấy danh sách không thành công!");
            }
        }

        public async Task<BaseResponse<UserViewModel>> GetAsync(string userId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(Guid.Parse(userId));
                if(user == null)
                    return BaseResponse<UserViewModel>.NotFound("Không tìm thấy thông tin!");

                var data = new UserViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                };

                return BaseResponse<UserViewModel>.Success(data, "Lấy thông tin thành công!");
            }
            catch (Exception)
            {
                return BaseResponse<UserViewModel>.NotFound("Không tìm thấy thông tin!");
            }
        }

        public async Task<BaseResponse<LoginResult>> LoginAsync(LoginRequest request, IConfiguration configuration)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.FindAsync(x => x.UserName == request.UserName);

                if (user == null)
                    return BaseResponse<LoginResult>.NotFound("Không tìm thấy thông tin!");

                var checkPassword = PasswordExtension.VerifyPassword(request.Password, user.Password, user.Salt);

                if (!checkPassword)
                    return BaseResponse<LoginResult>.BadRequest("Mật khẩu không đúng!");

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
                    return BaseResponse<LoginResult>.BadRequest("Đăng nhập thất bại");

                var data = new LoginResult()
                {
                    UserInfor = new UserViewModel()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        FullName = user.FullName,
                    },
                    Token = new Token()
                    {
                        AccessToken = accessToken,
                        ExpireIn = expireIn,
                    }
                };

                return BaseResponse<LoginResult>.Success(data, "Đăng nhập thành công!");
            }
            catch (Exception)
            {
                return BaseResponse<LoginResult>.BadRequest("Đăng nhập thất bại");
            }
        }

        public async Task<BaseResponse<UserViewModel>> UpdateAsync(UpdateUserRequest request)
        {
            try
            {
                var user = _unitOfWork.UserRepository.GetById(request.Id);

                if(user == null)
                    return BaseResponse<UserViewModel>.NotFound("Không tìm thấy thông tin");

                user.UserName = request.UserName;
                user.Email = request.Email;
                user.FullName = request.FullName;

                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.SaveChangesAsync();

                var data = new UserViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email
                };

                return BaseResponse<UserViewModel>.Success(data, "Cập nhật thành công!");
            }
            catch (Exception)
            {
                await _unitOfWork.CancelAsync();
                return BaseResponse<UserViewModel>.BadRequest("Cập nhật thất bại!");
            }
        }
    }
}
