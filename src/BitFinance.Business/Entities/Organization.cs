using BitFinance.Business.Enums;

namespace BitFinance.Business.Entities;

public class Organization
{
    private const string DefaultTimeZoneId = "America/Sao_Paulo";

    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string TimeZoneId { get; set; } = "America/Sao_Paulo";
    /// <summary>
    /// The subscription plan tier for this organization. Determines feature access and resource limits.
    /// </summary>
    public PlanTier PlanTier { get; set; } = PlanTier.Free;

    /// <summary>
    /// The members of this organization, including their roles.
    /// </summary>
    public ICollection<OrganizationMember> Members { get; set; } = new List<OrganizationMember>();

    /// <summary>
    /// Pending and historical invitations for this organization.
    /// </summary>
    public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public DateTime GetCurrentLocalTime()
    {
        var timeZone = ResolveTimeZone(TimeZoneId);
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
    }
    
    public DateOnly GetCurrentLocalDate()
    {
        return DateOnly.FromDateTime(GetCurrentLocalTime());
    }

    private static TimeZoneInfo ResolveTimeZone(string? timeZoneId)
    {
        foreach (var candidate in GetTimeZoneCandidates(timeZoneId))
        {
            if (TryResolveTimeZone(candidate, out var resolvedTimeZone))
            {
                return resolvedTimeZone;
            }
        }

        return TimeZoneInfo.Utc;
    }

    private static bool TryResolveTimeZone(string timeZoneId, out TimeZoneInfo resolvedTimeZone)
    {
        try
        {
            resolvedTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return true;
        }
        catch (TimeZoneNotFoundException)
        {
            resolvedTimeZone = TimeZoneInfo.Utc;
            return false;
        }
        catch (InvalidTimeZoneException)
        {
            resolvedTimeZone = TimeZoneInfo.Utc;
            return false;
        }
    }

    private static IEnumerable<string> GetTimeZoneCandidates(string? timeZoneId)
    {
        var normalizedTimeZoneId = NormalizeTimeZoneId(timeZoneId);

        if (!string.IsNullOrWhiteSpace(normalizedTimeZoneId))
            yield return normalizedTimeZoneId;

        if (!string.Equals(normalizedTimeZoneId, DefaultTimeZoneId, StringComparison.Ordinal))
            yield return DefaultTimeZoneId;

        if (!string.Equals(normalizedTimeZoneId, "UTC", StringComparison.OrdinalIgnoreCase))
            yield return "UTC";
    }

    private static string NormalizeTimeZoneId(string? timeZoneId)
    {
        return string.IsNullOrWhiteSpace(timeZoneId)
            ? string.Empty
            : timeZoneId.Trim().Replace(' ', '_');
    }
}
