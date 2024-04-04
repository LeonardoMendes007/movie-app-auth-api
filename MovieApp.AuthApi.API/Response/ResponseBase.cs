using System.Net;

namespace MovieApp.AuthApi.API.Response;

public class ResponseBase<T> : ResponseBase
{
    public T Data { get; set; }
    public ResponseBase(T data, HttpStatusCode statusCode) : base(statusCode)
    {
        Data = data;
    }
    public ResponseBase(T data, HttpStatusCode statusCode, string? message) : base(statusCode, message)
    {
        Data = data;
    }
}

public class ResponseBase
{
    public HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }

    public ResponseBase(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    public ResponseBase(HttpStatusCode statusCode, string? message)
    {
        StatusCode = statusCode;
        Message = message;
    }
}


