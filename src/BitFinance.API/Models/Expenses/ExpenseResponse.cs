using System.Text.Json.Serialization;
using BitFinance.Domain.Enums;

namespace BitFinance.API.Models.Expenses;

public record ExpenseResponse
{
    public Guid Id { get; set; }
    
    public string Description { get; set; } = null!;
    
    public decimal Amount { get; set; }
    
    public DateTime OccurredAt { get; set; }
    
    public string CreatedBy { get; set; } = null!;
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ExpenseCategory Category { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ExpenseStatus Status { get; set; }
}