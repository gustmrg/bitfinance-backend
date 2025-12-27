namespace BitFinance.Application.DTOs.Expenses;

public record CreateExpenseRequest(string Description, string Category, decimal Amount, string Status, DateTime? OccurredAt, string CreatedBy);
