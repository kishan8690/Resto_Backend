using System;

namespace Resto_Backend
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public object Data { get; set; }
        public string Message { get; set; } = "Success";
        public bool Success { get; set; }

        // Constructor
        public ApiResponse(int statusCode, object data, string message = "Success")
        {
            StatusCode = statusCode;
            Data = data;
            Message = message;
            Success = statusCode < 400;
        }
    }
}
