using System.Net;
using System.Text.Json;
using CityDiscoverTourist.Business.Helper.EmailHelper;
using Microsoft.AspNetCore.Http;

namespace CityDiscoverTourist.Business.Exceptions;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IEmailSender emailSender)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                case AppException:
                    // custom application error
                    response.StatusCode = (int) HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException:
                    // not found error
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
                case UnauthorizedAccessException:
                    // unauthorized error
                    response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(new
            {
                statusCode = response.StatusCode,
                message = error.Message,
                error = error.StackTrace
            });
            await response.WriteAsync(result);
        }
    }
}