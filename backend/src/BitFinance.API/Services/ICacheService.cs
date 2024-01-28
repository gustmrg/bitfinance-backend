namespace BitFinance.API.Services;

public interface ICacheService
{
    Task<string> GetAsync(string key);
    Task SetAsync(string key, string value);
    Task RemoveAsync(string key);
    string GenerateKey<T>(string entityId);
}