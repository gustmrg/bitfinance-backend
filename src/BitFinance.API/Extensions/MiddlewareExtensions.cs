using BitFinance.API.Middlewares;
using Microsoft.AspNetCore.HttpOverrides;

namespace BitFinance.API.Extensions;

public static class MiddlewareExtensions
{
    public static IServiceCollection AddHttpOptions(this IServiceCollection services)
    {
        services.AddCors();
        
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
        
        return services;
    }
    
    public static WebApplication ConfigureMiddleware(this WebApplication app, IConfiguration configuration)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors(options =>
        {
            var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
            
            options.WithOrigins(allowedOrigins ?? [])
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
        
        app.UseForwardedHeaders();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.ApplyMigrations();
        }

        if (app.Environment.IsProduction())
        {
            app.UseHsts();
        }
        
        app.UseHttpsRedirection();
        app.MapControllers();
        
        return app;
    }
}