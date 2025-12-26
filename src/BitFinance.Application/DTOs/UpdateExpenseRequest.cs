namespace BitFinance.Application.DTOs;

public record UpdateExpenseRequest(string Description, string Category, decimal Amount, string Status, DateTime? OccurredAt);