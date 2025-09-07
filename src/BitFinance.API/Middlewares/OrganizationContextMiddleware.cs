using System.Security.Claims;
using BitFinance.Application.Interfaces;

namespace BitFinance.API.Middlewares;

public class OrganizationContextMiddleware
{
    private readonly RequestDelegate _next;

    public OrganizationContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context, IUserSessionService sessionService)
    {
        try
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    var session = await sessionService.GetSessionAsync(userId);
                    if (session != null)
                    {
                        context.Items["CurrentOrganizationId"] = session.CurrentOrganizationId;
                    }
                    else
                    {
                        // Fallback to first organization from JWT
                        var orgsFromJwt = context.User.FindAll("organizations").FirstOrDefault()?.Value;
                        if (!string.IsNullOrEmpty(orgsFromJwt))
                        {
                            context.Items["CurrentOrganizationId"] = orgsFromJwt;
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            // Log error if needed, but don't let middleware fail the request
            // context.Items["CurrentOrganizationId"] will remain unset
        }

        await _next(context);
    }
}