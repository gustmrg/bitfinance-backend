using BitFinance.API.Middlewares;
using BitFinance.API.Services;
using BitFinance.Application.Interfaces;
using BitFinance.Application.Services;
using BitFinance.Application.Utils;
using BitFinance.Domain.Interfaces;
using BitFinance.Infrastructure.Repositories;
using BitFinance.Infrastructure.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace BitFinance.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddHostedService<BillStatusWorkerService>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IBillRepository, BillRepository>();
        services.AddScoped<IBillDocumentRepository, BillDocumentRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        // services.AddScoped<IOrganizationInvitesRepository, OrganizationInvitesRepository>();
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserSessionService, UserSessionService>();
        services.AddScoped<IBillService, BillService>();
        services.AddScoped<IExpenseService, ExpenseService>();
        services.AddScoped<IFileStorageService, LocalFileStorageService>();
        services.AddScoped<IFileValidationService, FileValidationService>();
        services.AddScoped<IBillDocumentService, BillDocumentService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        
        services.AddSingleton<CacheKeyBuilder>();
        
        services.AddSingleton<ICacheService, RedisCacheService>();
        services.AddSingleton<DistributedCacheEntryOptions>();
        services.AddTransient<GlobalExceptionHandlerMiddleware>();
        
        return services;
    }
}