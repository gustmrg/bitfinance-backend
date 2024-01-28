using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace BitFinance.API.Extensions;

public static class DistributedCacheExtensions
{
    public static string GenerateKey<T>(this IDistributedCache cache, string entityId)
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
    
    public static async Task SetValueAsync<T>(this IDistributedCache cache, 
        string entityId, 
        T data, 
        TimeSpan? absoluteExpireTime = null, 
        TimeSpan? unusedExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions();

        options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
        options.SlidingExpiration = unusedExpireTime;

        var jsonData = JsonSerializer.Serialize(data);
        await cache.SetStringAsync(entityId, jsonData, options);
    }

    public static async Task<T> GetValueAsync<T>(this IDistributedCache cache, string key)
    {
        var jsonData = await cache.GetStringAsync(key);

        if (jsonData is null)
        {
            return default(T);
        }

        return JsonSerializer.Deserialize<T>(jsonData);
    }

    public static async Task RemoveValueAsync(this IDistributedCache cache, string key)
    {
        await cache.RemoveAsync(key);
    }
}