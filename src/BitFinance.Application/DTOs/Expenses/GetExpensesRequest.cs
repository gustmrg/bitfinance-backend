namespace BitFinance.Application.DTOs.Expenses;

public record GetExpensesRequest(Guid OrganizationId, int Page = 1, int PageSize = 20);
