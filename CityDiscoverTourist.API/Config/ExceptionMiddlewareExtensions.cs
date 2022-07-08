using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Config;

/// <summary>
/// </summary>
public static class ExceptionMiddlewareExtensions
{
    /// <summary>
    /// </summary>
    /// <param name="app"></param>
    /// <param name="isDevelopment"></param>
    public static void HandlerExceptionProduction(this IApplicationBuilder app, bool isDevelopment)
    {
        if (!isDevelopment)
            // Do not add exception handler for dev environment. In dev,
            // we get the developer exception page with detailed error info.
            app.UseExceptionHandler(errorApp =>
            {
                // Logs unhandled exceptions. For more information about all the
                // different possibilities for how to handle errors see
                // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-5.0
                errorApp.Run(async context =>
                {
                    // Return machine-readable problem details. See RFC 7807 for details.
                    // https://datatracker.ietf.org/doc/html/rfc7807#page-6
                    var pd = new ProblemDetails
                    {
                        Title = "An unexpected error occurred!",
                        Status = context.Response.StatusCode,
                        Detail = context.Features.Get<IExceptionHandlerFeature>()?.Error.Message,
                        Instance = context.Request.Path.Value
                    };
                    pd.Extensions.Add("RequestId", context.TraceIdentifier);
                    pd.Extensions.Add("UserId", context.User.Identity!.Name ?? "Anonymous");
                    await context.Response.WriteAsJsonAsync(pd, pd.GetType(), null, "application/problem+json");
                });
            });
    }
}