namespace BitFinance.Application.DTOs;

public record GetBillsRequest(int Page = 1, int PageSize = 20);