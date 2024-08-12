using App.Application.UseCases.Contracts;
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

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest payload)
        {
            var result = await _userService.ChangePasswordAsync(payload);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(GetListRequest payload)
        {
            var result = await _userService.GetAllAsync(payload);
            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            var result = await _userService.GetAsync(userId);
            return Ok(result);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> Update(UpdateUserRequest payload, string userId)
        {
            if(payload.Id != Guid.Parse(userId))
            {
                return BadRequest("ID không khớp!");
            }

            var result = await _userService.UpdateAsync(payload);
            return Ok(result);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            var result = await _userService.DeleteAsync(userId);
            return Ok(result);
        }
    }
}
