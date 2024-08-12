using App.Common.Bases;
using App.Common.Models.User.Dtos;
using App.Common.Models.User.Request;
using App.Common.Models.User.Result;
using Microsoft.Extensions.Configuration;

namespace App.Application.UseCases.Contracts
{
    public interface IUserService
    {
        Task<BaseResponse<IEnumerable<UserViewModel>>> GetAllAsync();
        Task<BaseResponse<UserViewModel>> GetAsync();
        Task<BaseResponse<bool>> DeleteAsync(string UserId);
        Task<BaseResponse<UserViewModel>> CreateUserAsync(CreateUserRequest request);
        Task<BaseResponse<LoginResult>> LoginAsync(LoginRequest request, IConfiguration configuration);
        Task<BaseResponse<bool>> ChangePasswordAsync(ChangePasswordRequest request);
    }
}
