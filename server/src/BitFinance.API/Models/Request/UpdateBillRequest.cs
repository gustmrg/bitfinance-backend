using BitFinance.Business.Enums;

namespace BitFinance.API.Models.Request;

public class UpdateBillRequest
{
    public string Name { get; set; } = null!;
    public BillCategory Category { get; set; }
    public BillStatus Status { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public decimal AmountDue { get; set; }
    public decimal? AmountPaid { get; set; }
}