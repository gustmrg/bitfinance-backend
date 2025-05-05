using BitFinance.API.Middlewares;

namespace BitFinance.API.Extensions;

public static class OrganizationAuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder UseOrganizationAuthorization(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<OrganizationAuthorizationMiddleware>();
    }
}