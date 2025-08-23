using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Azure.Security.KeyVault.Secrets;

namespace BitFinance.API.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtKey = configuration["Jwt--Key"] ?? configuration["Jwt:Key"];
                var jwtIssuer = configuration["Jwt--Issuer"] ?? configuration["Jwt:Issuer"];
                var jwtAudience = configuration["Jwt--Audience"] ?? configuration["Jwt:Audience"];
                
                if (string.IsNullOrWhiteSpace(jwtKey))
                    throw new InvalidOperationException("JWT Key not found in configuration or Key Vault");
                if (string.IsNullOrWhiteSpace(jwtIssuer))
                    throw new InvalidOperationException("JWT Issuer not found in configuration or Key Vault");
                if (string.IsNullOrWhiteSpace(jwtAudience))
                    throw new InvalidOperationException("JWT Audience not found in configuration or Key Vault");
                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });
        
        return services;
    }
    
    private static string GetJwtKey(IServiceCollection services, IConfiguration configuration)
    {
        return GetSecretFromKeyVaultOrFallback(services, "Jwt--Key", configuration["Jwt:Key"]);
    }
    
    private static string GetJwtIssuer(IServiceCollection services, IConfiguration configuration)
    {
        return GetSecretFromKeyVaultOrFallback(services, "Jwt--Issuer", configuration["Jwt:Issuer"]);
    }
    
    private static string GetJwtAudience(IServiceCollection services, IConfiguration configuration)
    {
        return GetSecretFromKeyVaultOrFallback(services, "Jwt--Audience", configuration["Jwt:Audience"]);
    }
    
    private static string GetSecretFromKeyVaultOrFallback(IServiceCollection services, string secretName, string? fallbackValue)
    {
        try
        {
            var serviceProvider = services.BuildServiceProvider();
            var secretClient = serviceProvider.GetService<SecretClient>();
            
            if (secretClient != null)
            {
                var secret = secretClient.GetSecret(secretName);
                if (secret?.Value?.Value != null)
                {
                    return secret.Value.Value;
                }
            }
        }
        catch
        {
            // Fall through to use fallback value
        }
        
        return fallbackValue ?? throw new InvalidOperationException($"JWT secret '{secretName}' not found in Azure Key Vault and no fallback value configured.");
    }
}