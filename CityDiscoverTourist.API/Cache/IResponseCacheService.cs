namespace CityDiscoverTourist.API.Cache;

/// <summary>
/// </summary>
public interface IResponseCacheService
{
    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="timeToLive"></param>
    /// <returns></returns>
    Task CacheResponseAsync(string key, object? value, TimeSpan timeToLive);

    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<string> GetCacheResponseAsync(string key);
}