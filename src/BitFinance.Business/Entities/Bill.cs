using BitFinance.Business.Enums;

namespace BitFinance.Business.Entities;

public class Bill
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public BillCategory Category { get; set; }
    public BillStatus Status { get; set; }
    public decimal AmountDue { get; set; }
    public decimal? AmountPaid { get; set; }
    public DateOnly DueDate { get; set; }
    public DateTimeOffset? PaymentDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
    public ICollection<BillDocument> Documents { get; set; } = new List<BillDocument>();
}