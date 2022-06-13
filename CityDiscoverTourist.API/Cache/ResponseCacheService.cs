using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CityDiscoverTourist.API.Cache;

/// <summary>
/// </summary>
public class ResponseCacheService : IResponseCacheService
{
    private readonly IDistributedCache _distributedCache;

    /// <summary>
    /// </summary>
    /// <param name="distributedCache"></param>
    public ResponseCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="timeToLive"></param>
    public async Task CacheResponseAsync(string key, object? value, TimeSpan timeToLive)
    {
        if (value == null) return;

        var serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        var serializedResponse = JsonConvert.SerializeObject(value, serializerSettings);

        await _distributedCache.SetStringAsync(key, serializedResponse, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = timeToLive
        });
    }

    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<string> GetCacheResponseAsync(string key)
    {
        var cacheReponse = await _distributedCache.GetStringAsync(key);
        return (string.IsNullOrEmpty(cacheReponse) ? null : cacheReponse) ?? string.Empty;
    }
}