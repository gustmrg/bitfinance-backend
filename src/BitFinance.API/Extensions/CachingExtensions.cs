using BitFinance.Application.Interfaces;
using BitFinance.Application.Utils;
using BitFinance.Infrastructure.Services;
using StackExchange.Redis;

namespace BitFinance.API.Extensions;

public static class CachingExtensions
{
    public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Cache");
        
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Cache connection string not found in configuration or Key Vault");
        
        services.AddSingleton<IConnectionMultiplexer>(provider => ConnectionMultiplexer.Connect(connectionString));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Cache");
            options.InstanceName = "BitFinance";
        });
        
        return services;
    }
}