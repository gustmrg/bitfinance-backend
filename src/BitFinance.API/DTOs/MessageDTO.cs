using BitFinance.Business.Enums;

namespace BitFinance.API.DTOs;

public class MessageDTO
{
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public MessagePlatform Platform { get; set; }
    public DateTime? ScheduledDate { get; set; }
}