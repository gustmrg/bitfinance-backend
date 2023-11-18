using BitFinance.API.Enums;

namespace BitFinance.API.Models;

public class Message
{
    public Guid Id { get; set; }
    public string From { get; set; } = null!;
    public string To { get; set; } = null!;
    public string? Body { get; set; }
    public MessagePlatform Platform { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public bool IsSent { get; set; }
    public bool? IsDeleted { get; set; }
}