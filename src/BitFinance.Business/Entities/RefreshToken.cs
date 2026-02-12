namespace BitFinance.Business.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }

    public string TokenHash { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public User User { get; set; } = null!;

    public Guid TokenFamilyId { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime? RevokedAt { get; set; }

    public string? RevokedReason { get; set; }

    public Guid? ReplacedByTokenId { get; set; }

    public RefreshToken? ReplacedByToken { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? CreatedByIp { get; set; }

    public string? UserAgent { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    public bool IsActive => !IsRevoked && !IsExpired;
}
