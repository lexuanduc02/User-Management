using App.Application.Services.Contracts;
using App.Common.Models.User.Request;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest payload)
        {
            var result = await _userService.CreateUserAsync(payload);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest payload)
        {
            var result = await _userService.LoginAsync(payload, _configuration);

            return Ok(result);
        }
    }
}
