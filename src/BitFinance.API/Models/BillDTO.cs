using BitFinance.API.Enums;

namespace BitFinance.API.Models;

public class BillDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public BillCategory Category { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public decimal AmountDue { get; set; }
    public decimal? AmountPaid { get; set; }
    public bool IsPaid { get; set; }
    public bool? IsDeleted { get; set; }
}