namespace BitFinance.Business.Entities;

public class Organization
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string TimeZoneId { get; set; } = "America/Sao_Paulo";
    public ICollection<User> Members { get; set; } = new List<User>();
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public DateTime GetCurrentLocalTime()
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
    }
    
    public DateOnly GetCurrentLocalDate()
    {
        return DateOnly.FromDateTime(GetCurrentLocalTime());
    }
}