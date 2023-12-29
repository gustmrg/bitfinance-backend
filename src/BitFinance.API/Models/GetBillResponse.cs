using BitFinance.Business.Enums;

namespace BitFinance.API.Models;

public class GetBillResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public BillCategory Category { get; set; }
    public decimal AmountDue { get; set; }
    public decimal? AmountPaid { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public bool IsPaid { get; set; }
    public bool IsDeleted { get; set; }
}