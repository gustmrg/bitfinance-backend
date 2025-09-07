using Microsoft.Extensions.Configuration;

namespace BitFinance.Application.Utils;

public class CacheKeyBuilder
{
    private readonly string _keyPrefix;

    public CacheKeyBuilder(IConfiguration configuration)
    {
        var appName = configuration["ApplicationName"] ?? "bitfinance";
        _keyPrefix = $"{appName}";
    }
    
    public string ForUser(string userId) => $"{_keyPrefix}:user:{userId}";
    public string ForUserSession(string userId) => $"{_keyPrefix}:session:{userId}";
    public string ForBill(Guid billId) => $"{_keyPrefix}:bill:{billId}";
    public string ForExpense(Guid expenseId) => $"{_keyPrefix}:expense:{expenseId}";
    public string ForEntity<T>(string id) => $"{_keyPrefix}:{typeof(T).Name.ToLower()}:{id}";
}