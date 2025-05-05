namespace BitFinance.API.Models.Expenses;

public record GetExpensesRequest(int Page = 1, int PageSize = 100, DateTime? From = null, DateTime? To = null);