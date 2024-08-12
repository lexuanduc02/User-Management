namespace App.Common.Models.User.Request
{
    public class GetListRequest
    {
        public string Keyword { get; set; } = string.Empty;
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; init; } = 20;
    }
}
