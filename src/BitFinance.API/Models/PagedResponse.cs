namespace BitFinance.API.Models;

public class PagedResponse<T>(List<T> data, int totalRecords, int page, int pageSize)
{
    public List<T> Data { get; set; } = data;
    public int TotalRecords { get; set; } = totalRecords;
    public int Page { get; set; } = page;
    public int PageSize { get; set; } = pageSize;
}