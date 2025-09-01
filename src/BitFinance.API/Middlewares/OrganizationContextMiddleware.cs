using System.Security.Claims;
using BitFinance.Application.Interfaces;

namespace BitFinance.API.Middlewares;

public class OrganizationContextMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IUserSessionService _sessionService;

    public OrganizationContextMiddleware(RequestDelegate next, IUserSessionService sessionService)
    {
        _next = next;
        _sessionService = sessionService;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                var session = await _sessionService.GetSessionAsync(userId);
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

        await _next(context);
    }
}