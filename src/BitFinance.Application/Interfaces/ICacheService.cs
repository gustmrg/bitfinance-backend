namespace BitFinance.Application.Interfaces;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) 
        where T : class;

    Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default)
        where T : class;
    
    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) 
        where T : class;
    
    Task SetAsync<T>(string key, T value, TimeSpan expirationTime, CancellationToken cancellationToken = default) 
        where T : class;
    
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default);
    
    Task<IDictionary<string, T?>> GetManyAsync<T>(IEnumerable<string> keys, CancellationToken cancellationToken = default) where T : class;
}