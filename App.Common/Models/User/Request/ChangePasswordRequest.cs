using System.ComponentModel.DataAnnotations;

namespace App.Common.Models.User.Request
{
    public class ChangePasswordRequest
    {
        [Required]
        public string Email { get; set; }
    }
}
