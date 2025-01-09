namespace BitFinance.API.Models.Request;

public record GetBillsRequest(Guid OrganizationId, int Page = 1, int PageSize = 20);