using CityDiscoverTourist.API.Cache;
using CityDiscoverTourist.Business.Settings;

namespace CityDiscoverTourist.API.Config;

public static class CacheConfig
{
    public static void SetUpCache(this IServiceCollection services, IConfiguration configuration)
    {
        var redisCache = new RedisCacheSetting();
        configuration.GetSection(nameof(RedisCacheSetting)).Bind(redisCache);
        services.AddSingleton(redisCache);

        services.AddStackExchangeRedisCache(op => op.Configuration = redisCache.ConnectionString);
        services.AddSingleton<IResponseCacheService, ResponseCacheService>();
    }
}