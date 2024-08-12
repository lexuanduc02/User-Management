using Newtonsoft.Json;

namespace App.Common.Bases
{
    public class BaseResponse<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("code")]
        public int ErrorCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        public BaseResponse()
        {
            Success = false;
            ErrorCode = 0;
            Message = "";
            Data = default(T);
        }

        public static BaseResponse<T> CreateErrorResponse(int errorCode, string message = "")
        {
            return new BaseResponse<T>
            {
                Success = false,
                Message = (string.IsNullOrEmpty(message) ? "Error!" : message),
                ErrorCode = errorCode,
                Data = default(T)
            };
        }

        public static BaseResponse<T> CreateErrorResponse(int errorCode, T data, string message = "")
        {
            return new BaseResponse<T>
            {
                Success = false,
                Message = (string.IsNullOrEmpty(message) ? "Error!" : message),
                ErrorCode = errorCode,
                Data = data
            };
        }

        public static BaseResponse<T> CreateErrorResponse(string message)
        {
            return CreateErrorResponse(400, message);
        }

        public static BaseResponse<T> CreateErrorResponse(Exception ex)
        {
            return CreateErrorResponse(400, ex.Message + " - Error in: " + ex.StackTrace);
        }

        public static BaseResponse<T> CreateErrorResponseForInvalidRequest()
        {
            return CreateErrorResponse(400, "Invalid request!");
        }

        public static BaseResponse<T> CreateSuccessResponse(int errorCode, T data, string message = "")
        {
            return new BaseResponse<T>
            {
                Success = true,
                Message = string.IsNullOrEmpty(message) ? "Success!" : message,
                ErrorCode = errorCode,
                Data = data
            };
        }

        public static BaseResponse<T> CreateSuccessResponse(int errorCode, string message)
        {
            return new BaseResponse<T>
            {
                Success = true,
                Message = string.IsNullOrEmpty(message) ? "Success!" : message,
                ErrorCode = errorCode
            };
        }

        public static BaseResponse<T> CreateSuccessResponse(T data, string message = "")
        {
            return CreateSuccessResponse(200, data, message);
        }

        public static BaseResponse<T> CreateSuccessResponse(string message)
        {
            return CreateSuccessResponse(200, message);
        }

        public static BaseResponse<T> CreateSuccessResponse()
        {
            return CreateSuccessResponse("Success!");
        }
    }
}
