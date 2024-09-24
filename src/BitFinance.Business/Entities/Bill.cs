using BitFinance.Business.Enums;

namespace BitFinance.Business.Entities;

public class Bill
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public BillCategory Category { get; set; }
    public BillStatus Status { get; set; }
    public decimal AmountDue { get; set; }
    public decimal? AmountPaid { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public DateTime? DeletedDate { get; set; }
}