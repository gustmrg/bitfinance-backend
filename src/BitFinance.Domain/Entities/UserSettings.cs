namespace BitFinance.Domain.Entities;

public class UserSettings
{
    public string UserId { get; set; }
    public string? PreferredLanguage { get; set; } = "en-US";
    public string? TimeZoneId { get; set; } = "America/Sao_Paulo";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public User User { get; set; }
}