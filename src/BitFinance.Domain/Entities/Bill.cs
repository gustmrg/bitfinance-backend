using BitFinance.Domain.Common;

namespace BitFinance.Domain.Entities;

public class Bill : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Category { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public decimal AmountDue { get; set; }
    public decimal? AmountPaid { get; set; }
    public bool IsPaid { get; set; }
    public bool? IsDeleted { get; set; }
}