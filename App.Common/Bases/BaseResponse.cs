using System.Net;

namespace App.Common.Bases
{
    public class BaseResponse<T>
    {
        public bool Succeeded { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public BaseResponse(){}

        public BaseResponse(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public BaseResponse(string message)
        {
            Succeeded = false;
            Message = message;
        }
        public BaseResponse(string message, bool succeeded)
        {
            Succeeded = succeeded;
            Message = message;
        }

        public static BaseResponse<T> Deleted(string message = null)
        {
            return new BaseResponse<T>()
            {
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = message
            };
        }

        public static BaseResponse<T> Success(T data, string message = null)
        {
            return new BaseResponse<T>()
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = string.IsNullOrWhiteSpace(message) ? "Success" : message,
            };
        }

        public static BaseResponse<T> Unauthorized()
        {
            return new BaseResponse<T>()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Succeeded = true,
                Message = "UnAuthorized"
            };
        }

        public static BaseResponse<T> BadRequest(string message = null, List<string> errors = null)
        {
            return new BaseResponse<T>()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = string.IsNullOrWhiteSpace(message) ? "BadRequest" : message,
            };
        }

        public static BaseResponse<T> Conflict(string message = null)
        {
            return new BaseResponse<T>()
            {
                StatusCode = HttpStatusCode.Conflict,
                Succeeded = false,
                Message = string.IsNullOrWhiteSpace(message) ? "Conflict" : message
            };
        }

        public static BaseResponse<T> UnprocessableContent(string message = null)
        {
            return new BaseResponse<T>()
            {
                StatusCode = HttpStatusCode.UnprocessableContent,
                Succeeded = false,
                Message = string.IsNullOrWhiteSpace(message) ? "UnprocessableContent" : message
            };
        }

        public static BaseResponse<T> NotFound(string message = null)
        {
            return new BaseResponse<T>()
            {
                StatusCode = HttpStatusCode.NotFound,
                Succeeded = false,
                Message = string.IsNullOrWhiteSpace(message) ? "NotFound" : message
            };
        }

        public static BaseResponse<T> Created(T data, string message = null)
        {
            return new BaseResponse<T>()
            {
                Data = data,
                StatusCode = HttpStatusCode.Created,
                Succeeded = true,
                Message = string.IsNullOrWhiteSpace(message) ? "Created" : message,
            };
        }
    }
}
