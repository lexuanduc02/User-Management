using App.Common.Bases;
using App.Common.Models.Common;
using App.Common.Models.User.Dtos;
using App.Common.Models.User.Request;
using App.Common.Models.User.Result;

namespace App.ApiIntegration.Contracts
{
    public interface IUserApiClient
    {
        Task<BaseResponse<PagedList<UserViewModel>>> GetAllAsync(GetListRequest request);
        Task<BaseResponse<UserViewModel>> GetAsync(string id);
        Task<BaseResponse<Guid>> DeleteAsync(string userId);
        Task<BaseResponse<UserViewModel>> CreateUserAsync(CreateUserRequest request);
        Task<BaseResponse<LoginResult>> LoginAsync(LoginRequest request);
        Task<BaseResponse<Guid>> ChangePasswordAsync(ChangePasswordRequest request);
        Task<BaseResponse<UserViewModel>> UpdateAsync(UpdateUserRequest request);
    }
}
