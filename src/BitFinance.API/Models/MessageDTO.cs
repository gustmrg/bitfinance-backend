using BitFinance.API.Enums;

namespace BitFinance.API.Models;

public class MessageDTO
{
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public MessagePlatform Platform { get; set; }
    public DateTime? ScheduledDate { get; set; }
}