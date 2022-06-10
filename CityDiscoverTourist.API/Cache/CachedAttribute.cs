using System.Text;
using CityDiscoverTourist.Business.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CityDiscoverTourist.API.Cache;


/// <summary>
///
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CachedAttribute : Attribute, IAsyncActionFilter
{
    private readonly int _timeToLiveSeconds;

    /// <inheritdoc />
    public CachedAttribute(int timeToLiveSeconds)
    {
        _timeToLiveSeconds = timeToLiveSeconds;
    }

    /// <inheritdoc />
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cacheSetting = context.HttpContext.RequestServices.GetRequiredService<RedisCacheSetting>();

        if (!cacheSetting.Enabled)
        {
            await next();
            return;
        }

        var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
        var cacheKey = GenerateKeyFromRequest(context.HttpContext.Request);

        var cacheResponse = await cacheService.GetCacheResponseAsync(cacheKey);

        if (!string.IsNullOrEmpty(cacheResponse))
        {
            var rs = new ContentResult
            {
                Content = cacheResponse,
                ContentType = "application/json",
                StatusCode = 200
            };

            context.Result = rs;

            return;
        }

        var executedContext = await next();

        if (executedContext.Result is ObjectResult okObjectResult)
            await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value,
                TimeSpan.FromSeconds(_timeToLiveSeconds));
    }

    private static string GenerateKeyFromRequest(HttpRequest request)
    {
        var keyBuilder = new StringBuilder();
        keyBuilder.Append($"{request.Path}");

        foreach (var (key, value) in request.Query.OrderBy(x => x.Key)) keyBuilder.Append($"|{key}-{value}");

        return keyBuilder.ToString();
    }
}