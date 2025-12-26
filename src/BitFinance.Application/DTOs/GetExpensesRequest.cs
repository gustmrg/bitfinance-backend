namespace BitFinance.Application.DTOs;

public record GetExpensesRequest(Guid OrganizationId, int Page = 1, int PageSize = 20);