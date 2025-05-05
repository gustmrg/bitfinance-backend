namespace BitFinance.API.Models.Bills;

public record GetBillsRequest(int Page = 1, int PageSize = 100, DateTime? From = null, DateTime? To = null);