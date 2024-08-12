using System.ComponentModel.DataAnnotations;

namespace App.Common.Models.User.Request
{
    public class CreateUserRequest
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string FullName { get; set; }
    }
}
