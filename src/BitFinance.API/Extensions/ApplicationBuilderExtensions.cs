using Swashbuckle.AspNetCore.SwaggerUI;

namespace BitFinance.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }

    public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
    {
        app.UseCors(options => 
            options.WithOrigins("http://localhost:3000", "https://gustavomiranda.dev")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());

        return app;
    }
}