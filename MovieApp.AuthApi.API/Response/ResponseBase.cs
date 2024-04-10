using System.Net;

namespace MovieApp.AuthApi.API.Response;

public class ResponseBase<T> : ResponseBase
{
    public T Data { get; set; }
    private ResponseBase(T data, HttpStatusCode statusCode) : base(statusCode)
    {
        Data = data;
    }
    private ResponseBase(T data, HttpStatusCode statusCode, string? message) : base(statusCode, message)
    {
        Data = data;
    }

    public static ResponseBase<T> ResponseBaseFactory(T data, HttpStatusCode statusCode, string? message)
    {
        return new ResponseBase<T>(data, statusCode, message);
    }

    public static ResponseBase<T> ResponseBaseFactory(T data, HttpStatusCode statusCode)
    {
        return new ResponseBase<T>(data, statusCode);
    }

}

public class ResponseBase
{
    public HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }

    protected ResponseBase(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    protected ResponseBase(HttpStatusCode statusCode, string? message)
    {
        StatusCode = statusCode;
        Message = message;
    }

    public static ResponseBase ResponseBaseFactory(HttpStatusCode statusCode)
    {
        return new ResponseBase(statusCode);
    }

    public static ResponseBase ResponseBaseFactory(HttpStatusCode statusCode, string? message)
    {
        return new ResponseBase(statusCode, message);
    }
}


