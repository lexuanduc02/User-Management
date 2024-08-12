using System.ComponentModel.DataAnnotations;

namespace App.Common.Models.User.Request
{
    public class GetListRequest
    {
        public string Keyword { get; set; } = string.Empty;

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Value must be greater than 0.")]
        public int PageIndex { get; set; } = 1;

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Value must be greater than 0.")]
        public int PageSize { get; init; } = 20;
    }
}
