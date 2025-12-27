namespace BitFinance.Application.DTOs.Expenses;

public record UpdateExpenseRequest(string Description, string Category, decimal Amount, string Status, DateTime? OccurredAt);
