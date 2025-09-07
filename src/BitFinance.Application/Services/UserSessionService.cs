using System.Text.Json;
using BitFinance.Application.Interfaces;
using BitFinance.Application.Models;
using BitFinance.Application.Utils;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace BitFinance.Application.Services;

public class UserSessionService : IUserSessionService
{
    private readonly IDatabase _redis;
    private readonly ILogger<UserSessionService> _logger;
    private readonly CacheKeyBuilder _keyBuilder;
    private readonly TimeSpan _sessionExpiration = TimeSpan.FromDays(30);

    public UserSessionService(IConnectionMultiplexer redis,
        CacheKeyBuilder keyBuilder,
        ILogger<UserSessionService> logger)
    {
        _redis = redis.GetDatabase();
        _keyBuilder = keyBuilder;
        _logger = logger;
    }

    public async Task<UserSession?> GetSessionAsync(string userId)
    {
        try
        {
            var key = _keyBuilder.ForUserSession(userId);
            
            var sessionJson =  await _redis.StringGetAsync(key);

            if (!sessionJson.HasValue)
                return null;
            
            return JsonSerializer.Deserialize<UserSession>(sessionJson);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving session for user {UserId}", userId);
            return null;
        }
    }

    public async Task SetCurrentOrganizationAsync(string userId, Guid organizationId)
    {
        try
        {
            var session = new UserSession
            {
                UserId = userId,
                CurrentOrganizationId = organizationId.ToString(),
                LastUpdated = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.Add(_sessionExpiration)
            };
            
            var key = _keyBuilder.ForUserSession(userId);
            var sessionJson = JsonSerializer.Serialize(session);
            
            await _redis.StringSetAsync(key, sessionJson, _sessionExpiration);
            
            _logger.LogInformation("Session updated for user {UserId} to organization {OrgId}", userId, organizationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting session for user {UserId}", userId);
            throw;
        }
    }

    public async Task ClearSessionAsync(string userId)
    {
        try
        {
            var key = _keyBuilder.ForUserSession(userId);
            await _redis.KeyDeleteAsync(key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing session for user {UserId}", userId);
        }
    }
}