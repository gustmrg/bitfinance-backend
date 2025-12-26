using System.Text.Json.Serialization;
using BitFinance.Domain.Enums;

namespace BitFinance.Application.DTOs;

public class CreateExpenseResponse
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ExpenseCategory Category { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ExpenseStatus Status { get; set; }
    public decimal Amount { get; set; }
    public DateTimeOffset OccurredAt { get; set; }
    public string CreatedBy { get; set; } = null!;
}