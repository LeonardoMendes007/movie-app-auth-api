using MovieApp.AuthApi.API.Response;
using MovieApp.AuthApi.Identity.Exceptions;
using System.Net;

namespace MovieApp.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unexpected error occurred.");
     

        ResponseBase response = exception switch
        {
            UnauthorizedAccessException _ => ResponseBase.ResponseBaseFactory(HttpStatusCode.Unauthorized, "Unauthorized."),
            UserNameAlreadyExistsException _ => ResponseBase.ResponseBaseFactory(HttpStatusCode.BadRequest, exception.Message),
            EmailAlreadyExistsException _ => ResponseBase.ResponseBaseFactory(HttpStatusCode.BadRequest, exception.Message),
            InvalidRefreshTokenException _ => ResponseBase.ResponseBaseFactory(HttpStatusCode.BadRequest, exception.Message),
            _ => ResponseBase.ResponseBaseFactory(HttpStatusCode.InternalServerError, "Internal server error. Please retry later.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}
