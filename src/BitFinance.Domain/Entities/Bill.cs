using BitFinance.Domain.Enums;
using BitFinance.Domain.Exceptions;
using BitFinance.Domain.ValueObjects;

namespace BitFinance.Domain.Entities;

public class Bill
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public BillCategory Category { get; set; }
    public BillStatus Status { get; set; }
    public Money AmountDue { get; set; }
    public Money? AmountPaid { get; set; }
    public DateOnly DueDate { get; set; }
    public DateTimeOffset? PaymentDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
    public ICollection<BillDocument> Documents { get; set; } = new List<BillDocument>();
    
    public void MarkAsPaid(Money amount, DateTimeOffset paymentDate)
    {
        if (Status == BillStatus.Paid)
            throw new DomainException("Bill is already paid");
        
        AmountPaid = amount;
        PaymentDate = paymentDate;
        Status = BillStatus.Paid;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void MarkAsOverdue()
    {
        if (Status is BillStatus.Paid or BillStatus.Cancelled) return;
        Status = BillStatus.Overdue;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsOverdue() =>
        DueDate < DateOnly.FromDateTime(DateTime.UtcNow)
        && Status is not (BillStatus.Paid or BillStatus.Cancelled);
}