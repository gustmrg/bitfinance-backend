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
                var jwtKey = configuration["Jwt:Key"];
                var jwtIssuer = configuration["Jwt:Issuer"];
                var jwtAudience = configuration["Jwt:Audience"];
                
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
}