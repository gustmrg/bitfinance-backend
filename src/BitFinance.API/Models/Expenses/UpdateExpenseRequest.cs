namespace BitFinance.API.Models.Expenses;

public record UpdateExpenseRequest(string Description, string Category, decimal Amount, string Status, DateTime? OccurredAt);