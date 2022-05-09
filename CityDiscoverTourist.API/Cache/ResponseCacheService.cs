using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CityDiscoverTourist.API.Cache;

public class ResponseCacheService : IResponseCacheService
{
    private readonly IDistributedCache _distributedCache;

    public ResponseCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task CacheResponseAsync(string key, object? response, TimeSpan timeToLive)
    {
        if (response == null) return;

        var serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        var serializedResponse = JsonConvert.SerializeObject(response, serializerSettings);

        await _distributedCache.SetStringAsync(key, serializedResponse, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = timeToLive
        });
    }

    public async Task<string> GetCacheResponseAsync(string key)
    {
        var cacheReponse = await _distributedCache.GetStringAsync(key);
        return (string.IsNullOrEmpty(cacheReponse) ? null : cacheReponse) ?? string.Empty;
    }
}