using Amazon.S3;
using BitFinance.API.Middlewares;
using BitFinance.API.Services;
using BitFinance.API.Services.Interfaces;
using BitFinance.API.Settings;
using BitFinance.Business.Interfaces;
using BitFinance.Data.Caching;
using BitFinance.Data.Repositories;
using BitFinance.Data.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace BitFinance.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<BillStatusWorkerService>();
        services.AddHostedService<RefreshTokenCleanupService>();

        services.AddScoped<IBillsRepository, BillsRepository>();
        services.AddScoped<IOrganizationsRepository, OrganizationsRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IExpensesRepository, ExpensesRepository>();
        services.AddScoped<IInvitationsRepository, InvitationsRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IBillDocumentsRepository, BillDocumentsRepository>();

        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IBillsService, BillsService>();
        services.AddScoped<IExpensesService, ExpensesService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IFileValidationService, FileValidationService>();
        services.AddScoped<IBillDocumentService, BillDocumentService>();
        services.AddScoped<ICookieService, CookieService>();

        services.AddSingleton<IValidateOptions<JwtSettings>, JwtSettingsValidator>();
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(JwtSettings.SectionName))
            .ValidateOnStart();

        services.AddSingleton<IValidateOptions<StorageSettings>, StorageSettingsValidator>();
        services.AddOptions<StorageSettings>()
            .Bind(configuration.GetSection(StorageSettings.SectionName))
            .ValidateOnStart();
        services.AddFileStorageProvider(configuration);

        services.AddSingleton<ICacheService, RedisCacheService>();
        services.AddSingleton<DistributedCacheEntryOptions>();
        services.AddTransient<GlobalExceptionHandlerMiddleware>();

        return services;
    }

    private static IServiceCollection AddFileStorageProvider(this IServiceCollection services, IConfiguration configuration)
    {
        var provider = configuration.GetValue<string>("Storage:Provider") ?? "Local";

        switch (provider)
        {
            case "S3":
                var region = configuration.GetValue<string>("Storage:S3:Region") ?? "us-east-1";
                services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(Amazon.RegionEndpoint.GetBySystemName(region)));
                services.AddScoped<IFileStorageService, S3FileStorageService>();
                break;
            default:
                services.AddScoped<IFileStorageService, LocalFileStorageService>();
                break;
        }

        return services;
    }
}