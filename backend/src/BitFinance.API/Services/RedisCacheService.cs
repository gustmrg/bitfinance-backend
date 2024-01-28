using System.Text;
using Microsoft.Extensions.Caching.Distributed;

namespace BitFinance.API.Services;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly DistributedCacheEntryOptions _options;

    public RedisCacheService(IDistributedCache cache, DistributedCacheEntryOptions options)
    {
        _cache = cache;
        _options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3660),
            SlidingExpiration = TimeSpan.FromSeconds(1200)
        };
    }
    
    public async Task<string> GetAsync(string key)
    {
        return await _cache.GetStringAsync(key);
    }

    public async Task SetAsync(string key, string value)
    {
        await _cache.SetStringAsync(key, value, _options);
    }
    
    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }

    public string GenerateKey<T>(string entityId)
    {
        StringBuilder cacheKeyBuilder = new StringBuilder();
        var entityType = typeof(T);
        string entityNamespacePrefix = entityType.Namespace + "_";
        string assemblyVersionPrefix = entityType.Assembly.GetName().Version + "_";
        string entityTypeNamePrefix = entityType.Name + "_";
        cacheKeyBuilder.Append(entityNamespacePrefix);
        cacheKeyBuilder.Append(assemblyVersionPrefix);
        cacheKeyBuilder.Append(entityTypeNamePrefix);
        cacheKeyBuilder.Append(entityId);

        return cacheKeyBuilder.ToString();
    }
}