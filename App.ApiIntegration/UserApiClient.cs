using App.ApiIntegration.Contracts;
using App.Common.Bases;
using App.Common.Models.Common;
using App.Common.Models.User.Dtos;
using App.Common.Models.User.Request;
using App.Common.Models.User.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace App.ApiIntegration
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public UserApiClient(IHttpContextAccessor httpContextAccessor, 
            IHttpClientFactory httpClientFactory, 
            IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<BaseResponse<Guid>> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var accessToken = _httpContextAccessor.HttpContext.User.FindFirst("AccessToken").Value;

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddressUri"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.PutAsync($"/api/user/change-password", httpContent);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BaseResponse<Guid>>(result);
        }

        public async Task<BaseResponse<UserViewModel>> CreateUserAsync(CreateUserRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var accessToken = _httpContextAccessor.HttpContext.User.FindFirst("AccessToken").Value;

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddressUri"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.PostAsync($"/api/user/register", httpContent);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BaseResponse<UserViewModel>>(result);
        }

        public async Task<BaseResponse<Guid>> DeleteAsync(string userId)
        {
            var accessToken = _httpContextAccessor.HttpContext.User.FindFirst("AccessToken").Value;

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddressUri"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.DeleteAsync($"/api/user/{userId}");

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BaseResponse<Guid>>(result);
        }

        public async Task<BaseResponse<PagedList<UserViewModel>>> GetAllAsync(GetListRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var accessToken = _httpContextAccessor.HttpContext.User.FindFirst("AccessToken").Value;

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddressUri"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.PostAsync($"/api/user/", httpContent);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BaseResponse<PagedList<UserViewModel>>>(result);
        }

        public async Task<BaseResponse<UserViewModel>> GetAsync(string id)
        {
            var accessToken = _httpContextAccessor.HttpContext.User.FindFirst("AccessToken").Value;

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddressUri"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"/api/user/{id}");

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BaseResponse<UserViewModel>>(result);
        }

        public async Task<BaseResponse<LoginResult>> LoginAsync(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddressUri"]);

            var response = await client.PostAsync("/api/user/login", httpContent);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BaseResponse<LoginResult>>(result);
        }

        public async Task<BaseResponse<UserViewModel>> UpdateAsync(UpdateUserRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var accessToken = _httpContextAccessor.HttpContext.User.FindFirst("AccessToken").Value;

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddressUri"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.PutAsync($"/api/user/{request.Id}", httpContent);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BaseResponse<UserViewModel>>(result);
        }
    }
}
