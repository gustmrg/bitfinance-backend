using BitFinance.Application.Validators.Identity;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BitFinance.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

        return services;
    }
}
