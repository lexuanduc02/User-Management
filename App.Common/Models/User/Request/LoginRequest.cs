using System.ComponentModel.DataAnnotations;

namespace App.Common.Models.User.Request
{
    public class LoginRequest
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
