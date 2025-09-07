using Asp.Versioning;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace BitFinance.API.Extensions;

public static class DocumentationExtensions
{
    public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Enter your JWT token (without 'Bearer ' prefix)"
            });
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        // services.AddApiVersioning(options =>
        // {
        //     options.DefaultApiVersion = new ApiVersion(1, 0);
        //     options.ReportApiVersions = true;
        //     options.AssumeDefaultVersionWhenUnspecified = true;
        //     options.ApiVersionReader = ApiVersionReader.Combine(
        //         new UrlSegmentApiVersionReader(),
        //         new HeaderApiVersionReader("X-Api-Version"));
        // }).AddApiExplorer(options =>
        // {
        //     options.GroupNameFormat = "'v'V";
        //     options.SubstituteApiVersionInUrl = true;
        // });
        
        return services;
    }
}