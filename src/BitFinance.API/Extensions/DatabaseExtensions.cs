using BitFinance.Domain.Entities;
using BitFinance.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.API.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Database connection string not found in configuration or Key Vault.");
        
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(connectionString));

        services.AddIdentityCore<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddIdentityApiEndpoints<User>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        return services;
    }
}