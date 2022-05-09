namespace CityDiscoverTourist.API.Cache;

public interface IResponseCacheService
{
    Task CacheResponseAsync(string key, object? value, TimeSpan timeToLive);

    Task<string> GetCacheResponseAsync(string key);
}