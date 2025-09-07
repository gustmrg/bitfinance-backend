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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ClockSkew = TimeSpan.Zero
                };
                
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        Console.WriteLine($"JWT OnMessageReceived - Authorization header: {authHeader ?? "NULL"}");
                        
                        if (string.IsNullOrEmpty(authHeader))
                        {
                            Console.WriteLine("No Authorization header found");
                        }
                        else if (!authHeader.StartsWith("Bearer "))
                        {
                            Console.WriteLine($"Authorization header doesn't start with 'Bearer ': {authHeader}");
                        }
                        else
                        {
                            var token = authHeader.Substring("Bearer ".Length);
                            Console.WriteLine($"Token extracted: {token.Substring(0, Math.Min(50, token.Length))}...");
                        }
                        
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"JWT Authentication failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("JWT Token validated successfully");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        Console.WriteLine($"JWT Challenge: {context.Error} - {context.ErrorDescription}");
                        return Task.CompletedTask;
                    }
                };
            });
        
        return services;
    }
}