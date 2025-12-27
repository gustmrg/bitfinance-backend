using BitFinance.API.Middlewares;
using BitFinance.API.Services;
using BitFinance.Application.Interfaces;
using BitFinance.Application.Services;
using BitFinance.Domain.Interfaces.Repositories;
using BitFinance.Infrastructure.Caching;
using BitFinance.Infrastructure.FileStorage;
using BitFinance.Infrastructure.Identity;
using BitFinance.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Caching.Distributed;

namespace BitFinance.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddHostedService<BillStatusWorkerService>();
        
        services.AddScoped<IBillsRepository, BillsRepository>();
        services.AddScoped<IBillDocumentsRepository, BillDocumentsRepository>();
        services.AddScoped<IOrganizationsRepository, OrganizationsRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IExpensesRepository, ExpensesRepository>();
        services.AddScoped<IOrganizationInvitesRepository, OrganizationInvitesRepository>();
        
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IBillsService, BillsService>();
        services.AddScoped<IExpensesService, ExpensesService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IFileStorageService, LocalFileStorageService>();
        services.AddScoped<IFileValidationService, FileValidationService>();
        services.AddScoped<IBillDocumentService, BillDocumentService>();
        
        services.AddSingleton<ICacheService, RedisCacheService>();
        services.AddSingleton<DistributedCacheEntryOptions>();
        services.AddTransient<GlobalExceptionHandlerMiddleware>();
        
        return services;
    }
}