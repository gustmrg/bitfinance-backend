using System.Security.Claims;
using BitFinance.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BitFinance.API.Filters;

public class OrganizationAuthorizationFilter(IUsersService usersService) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var requestOrganizationId = context.RouteData.Values["organizationId"]?.ToString();

        if (string.IsNullOrEmpty(requestOrganizationId) || !Guid.TryParse(requestOrganizationId, out var organizationId))
        {
            context.Result = new BadRequestObjectResult("Organization Id is invalid.");
            return;
        }

        var userId = context.HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new UnauthorizedObjectResult("User is not authenticated.");
            return;
        }

        var result = await usersService.IsUserInOrganizationAsync(userId, organizationId);

        if (result.IsFailure || !result.Value)
        {
            context.Result = new ForbidResult("You do not have access to this organization.");
        }
    }
}
