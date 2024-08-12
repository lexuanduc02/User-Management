namespace App.Common.Bases
{
    public class BaseResponse<T>
    {
        public string? Message { get; set; }
        public int StatusCode { get; set; } = 0;
        public bool Success { get; set; }
        public T? Data { get; set; }
    }
}
