using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlogApi.Domain.Common
{
    

    public class Result<T> 
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public int StatusCode { get; set; }
        private Result(bool isSuccess, T? value,  int statusCode = 200) 
        {
            IsSuccess = isSuccess;
            Value = value;
            StatusCode = statusCode;
        }
        public static Result<T> Success(T value) => new(true, value);
        public static Result<T> Failure( int StatusCode = 400) => new(false, default, StatusCode);

        public static Result<T> NotFound() => new(false, default, 404);
        public static Result<T> Unauthorized() => new(false, default, 401);
        public static Result<T> Conflict() => new(false, default, 409);
        public static Result<T> Forbidden() => new(false, default, 403);
        public static Result<T> NoContent() => new(true, default, 204);
        public static Result<T> InternalServerError() => new(false, default, 500);


    }
}
