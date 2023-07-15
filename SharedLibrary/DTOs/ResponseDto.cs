using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs
{
    public class ResponseDto<T>
        where T : class
    {
        public T? Data { get; private set; }
        public int StatusCode { get; private set; }
        public ErrorDto? Error { get; private set; }

        [JsonIgnore]
        public bool IsSuccessful { get; private set; }
        public static ResponseDto<T> Sucess(T data, int statusCode)
            => new ResponseDto<T> { Data = data, StatusCode = statusCode, IsSuccessful=true };

        public static ResponseDto<T> Sucess(int statusCode)
            => new ResponseDto<T> { StatusCode = statusCode , IsSuccessful = true };

        public static ResponseDto<T> Fail(ErrorDto error, int statusCode)
            => new ResponseDto<T> { Error = error, StatusCode = statusCode, IsSuccessful = false };

        public static ResponseDto<T> Fail(string errorMessage, bool isShow, int statusCode)
        {
            ErrorDto errorDto = new(errorMessage, isShow);
            return new ResponseDto<T> { Error = errorDto, StatusCode = statusCode , IsSuccessful = false};
        }
    }
}
