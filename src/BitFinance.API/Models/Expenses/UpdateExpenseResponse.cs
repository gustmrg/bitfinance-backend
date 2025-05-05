using System.Text.Json.Serialization;
using BitFinance.Domain.Enums;

namespace BitFinance.API.Models.Expenses;

public record UpdateExpenseResponse(
    Guid Id, 
    string Description, 
    [property: JsonConverter(typeof(JsonStringEnumConverter))] ExpenseCategory Category, 
    decimal Amount,
    [property: JsonConverter(typeof(JsonStringEnumConverter))] ExpenseStatus Status,
    DateTime OccurredAt,
    string CreatedBy);