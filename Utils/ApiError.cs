using System;
using System.Collections.Generic;

namespace Resto_Backend
{
    public class ApiError : Exception
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = "Something went wrong";
        public bool Success { get; set; } = false;
        public List<string> Errors { get; set; } = new List<string>();
        public string Stack { get; set; }
        public object Data { get; set; } = null;

        public ApiError(int statusCode, string message = "Something went wrong", List<string> errors = null, string stack = null)
        {
            StatusCode = statusCode;
            Message = message;
            Success = false;
            Errors = errors ?? new List<string>();
            Stack = stack ?? Environment.StackTrace;
        }
    }
}
