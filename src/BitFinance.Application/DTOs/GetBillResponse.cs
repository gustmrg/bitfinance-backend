using System.Text.Json.Serialization;
using BitFinance.Domain.Enums;

namespace BitFinance.Application.DTOs;

public class GetBillResponse
{
    public Guid Id { get; set; }
    
    public string Description { get; set; } = null!;
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BillCategory Category { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BillStatus Status { get; set; }
    
    public decimal AmountDue { get; set; }
    
    public decimal? AmountPaid { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset DueDate { get; set; }

    public DateTimeOffset? PaymentDate { get; set; }
    public ICollection<DocumentSummary> Documents { get; set; } = new List<DocumentSummary>();
}