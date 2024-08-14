using App.ApiIntegration.Contracts;
using App.Common.Models.User.Dtos;
using App.Common.Models.User.Request;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.WebApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;

        public UserController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(CreateUserRequest payload)
        {
            if (!ModelState.IsValid)
                return View(payload);

            var result = await _userApiClient.CreateUserAsync(payload);

            if (!result.Succeeded)
            {
                return View(payload);
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _userApiClient.LoginAsync(request);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("LoginFailed", result.Message);
                return View(request);
            }

            var token = result.Data.Token;
            var userInfor = result.Data.UserInfor;

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, userInfor.Id.ToString()),
                new Claim(ClaimTypes.Email, userInfor.Email),
                new Claim("AccessToken", token.AccessToken)
            };

            var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(Double.Parse(token.ExpireIn)),
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Index(string keyWork = "", int pageIndex = 1, int pageSize = 20)
        {
            var payload = new GetListRequest()
            {
                Keyword = keyWork,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var data = await _userApiClient.GetAllAsync(payload);

            return View(data.Data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequest payload)
        {
            if (!ModelState.IsValid)
                return View(payload);

            var result = await _userApiClient.CreateUserAsync(payload);

            if(!result.Succeeded)
            {
                return View(payload);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userApiClient.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userApiClient.GetAsync(id);

            var model = new UpdateUserRequest()
            {
                Id = user.Data.Id,
                UserName = user.Data.UserName,
                FullName = user.Data.FullName,
                Email = user.Data.Email,
            };

            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserRequest payload)
        {
            if (!ModelState.IsValid)
                return View(payload);

            var result = await _userApiClient.UpdateAsync(payload);

            if (!result.Succeeded)
            {
                return View(payload);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest payload)
        {
            if (!ModelState.IsValid)
                return View(payload);

            var result = await _userApiClient.ChangePasswordAsync(payload);

            if (!result.Succeeded)
            {
                return View(payload);
            }

            return RedirectToAction(nameof(Login));
        }
    }
}
