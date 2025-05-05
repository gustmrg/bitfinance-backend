using System.Security.Claims;
using BitFinance.Domain.Services;

namespace BitFinance.API.Middlewares;

public class OrganizationAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<OrganizationAuthorizationMiddleware> _logger;
    
    public OrganizationAuthorizationMiddleware(RequestDelegate next, ILogger<OrganizationAuthorizationMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value;
        
        if (string.IsNullOrEmpty(path))
        {
            await _next(context);
            return;
        }
        
        if (!path.Contains("/organizations/", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }
        
        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        int orgIndex = Array.FindIndex(segments, s => s.Equals("organizations", StringComparison.OrdinalIgnoreCase));
            
        if (orgIndex < 0 || orgIndex >= segments.Length - 1)
        {
            _logger.LogWarning("Organization ID not found in the request path: {Path}", path);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { error = "Invalid organization route" });
            return;
        }
            
        string organizationId = segments[orgIndex + 1];
        
        var organizationService = context.RequestServices.GetRequiredService<IOrganizationsService>();
        
        bool organizationExists = await organizationService.ValidateOrganizationExistsAsync(Guid.Parse(organizationId));
        if (!organizationExists)
        {
            _logger.LogWarning("Organization not found: {OrganizationId}", organizationId);
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { error = "Organization not found" });
            return;
        }
        
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            _logger.LogWarning("Unauthenticated user attempted to access organization resource: {OrganizationId}", organizationId);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { error = "Authentication required" });
            return;
        }
        
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("User ID claim not found for authenticated user");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { error = "Invalid authentication" });
            return;
        }
        
        bool userBelongsToOrganization = await organizationService.ValidateUserBelongsToOrganizationAsync(userId, Guid.Parse(organizationId));
        if (!userBelongsToOrganization)
        {
            _logger.LogWarning("User {UserId} does not belong to organization {OrganizationId}", userId, organizationId);
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new { error = "You do not have access to this organization" });
            return;
        }
        
        context.Items["OrganizationId"] = organizationId;
        
        await _next(context);
    }
}