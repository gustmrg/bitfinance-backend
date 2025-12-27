namespace BitFinance.Application.DTOs.Bills;

public record GetBillsRequest(int Page = 1, int PageSize = 20);
