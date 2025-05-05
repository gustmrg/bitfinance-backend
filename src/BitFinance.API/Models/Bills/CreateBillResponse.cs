using System.Text.Json.Serialization;
using BitFinance.Domain.Enums;

namespace BitFinance.API.Models.Bills;

public class CreateBillResponse
{
    public Guid Id { get; set; }
    
    public string Description { get; set; } = null!;
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BillCategory Category { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BillStatus Status { get; set; }
    
    public decimal AmountDue { get; set; }
    
    public decimal? AmountPaid { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public DateTime DueDate { get; set; }
    
    public DateTime? PaidDate { get; set; }
}