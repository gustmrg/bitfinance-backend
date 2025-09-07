using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using BitFinance.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace BitFinance.Infrastructure.Services;

public class RedisCacheService : ICacheService
{
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();
    private readonly IDistributedCache _cache;
    private readonly DistributedCacheEntryOptions _options;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
        _options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
            SlidingExpiration = TimeSpan.FromSeconds(1800)
        };
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) 
        where T : class
    {
        string? cachedValue = await _cache.GetStringAsync(key, cancellationToken);

        if (cachedValue is null)
            return null;

        T? value = JsonSerializer.Deserialize<T>(cachedValue);

        return value;
    }

    public async Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default)
        where T : class
    {
        T? cachedValue = await GetAsync<T>(key, cancellationToken);

        if (cachedValue is not null)
            return cachedValue;

        cachedValue = await factory();

        await SetAsync(key, cachedValue, cancellationToken);
        
        return cachedValue;
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) 
        where T : class
    {
        string cacheValue = JsonSerializer.Serialize(value);

        await _cache.SetStringAsync(key, cacheValue, _options, cancellationToken);

        CacheKeys.TryAdd(key, true);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expirationTime, CancellationToken cancellationToken = default)
        where T : class
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expirationTime
        };
        
        string cacheValue = JsonSerializer.Serialize(value);

        await _cache.SetStringAsync(key, cacheValue,  options, cancellationToken);

        CacheKeys.TryAdd(key, true);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(key, cancellationToken);

        CacheKeys.TryRemove(key, out _);
    }

    public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
    {
        var tasks = CacheKeys.Keys
            .Where(k => k.StartsWith(prefixKey))
            .Select(k => RemoveAsync(k, cancellationToken));

        await Task.WhenAll(tasks);
    }

    public async Task<IDictionary<string, T?>> GetManyAsync<T>(IEnumerable<string> keys, CancellationToken cancellationToken = default) where T : class
    {
        var keysList = keys.ToList();
        var results = new Dictionary<string, T?>();
        
        var tasks = keysList.Select(async key =>
        {
            var value = await GetAsync<T>(key, cancellationToken);
            return new KeyValuePair<string, T?>(key, value);
        });
        
        var taskResults = await Task.WhenAll(tasks);
        
        foreach (var kvp in taskResults)
        {
            results[kvp.Key] = kvp.Value;
        }

        return results;
    }
}