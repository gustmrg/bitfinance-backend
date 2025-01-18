namespace BitFinance.API.Models.Request;

public record UpdateExpenseRequest(string Description, string Category, decimal Amount);