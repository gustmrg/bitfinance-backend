using System.Security.Claims;

namespace BitFinance.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;
    }
    
    public static string GetRequiredUserId(this ClaimsPrincipal user)
    {
        var userId = user.GetUserId();
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User ID not found in claims");
        
        return userId;
    }
}