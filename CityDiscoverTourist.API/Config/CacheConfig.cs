using CityDiscoverTourist.API.Cache;
using CityDiscoverTourist.Business.Settings;
using StackExchange.Redis;

namespace CityDiscoverTourist.API.Config;

/// <summary>
/// </summary>
public static class CacheConfig
{
    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void SetUpCache(this IServiceCollection services, IConfiguration configuration)
    {
        var redisCache = new RedisCacheSetting();
        configuration.GetSection(nameof(RedisCacheSetting)).Bind(redisCache);
        services.AddSingleton(redisCache);

        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisCache.ConnectionString));
        services.AddStackExchangeRedisCache(op => op.Configuration = redisCache.ConnectionString);
        services.AddSingleton<IResponseCacheService, ResponseCacheService>();
    }
}