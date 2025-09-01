namespace BitFinance.API.Extensions;

public static class HttpContextExtensions
{
    public static string GetCurrentOrganizationId(this HttpContext context)
    {
        return context.Items["CurrentOrganizationId"]?.ToString();
    }
}