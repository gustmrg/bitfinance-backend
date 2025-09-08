using BitFinance.Domain.Enums;

namespace BitFinance.Application.DTOs.Bills;

public class BillDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public BillCategory Category { get; set; }
    public BillStatus Status { get; set; }
    public decimal AmountDue { get; set; }
    public decimal? AmountPaid { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaymentDate { get; set; }
    // public Guid OrganizationId { get; set; }
    // public Organization Organization { get; set; } = null!;
    // public ICollection<BillDocument> Documents { get; set; } = new List<BillDocument>();
}