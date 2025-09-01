namespace BitFinance.Infrastructure.Models;

public class UserSession
{
    public string UserId { get; set; }
    public string CurrentOrganizationId { get; set; }
    public DateTime LastUpdated { get; set; }
    public DateTime ExpiresAt { get; set; }
}