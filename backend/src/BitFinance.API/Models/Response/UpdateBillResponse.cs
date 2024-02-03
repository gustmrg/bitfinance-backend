using BitFinance.Business.Enums;

namespace BitFinance.API.Models.Response;

public class UpdateBillResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public BillCategory Category { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public decimal AmountDue { get; set; }
    public decimal? AmountPaid { get; set; }
    public bool IsPaid { get; set; }
    public bool? IsDeleted { get; set; }
}