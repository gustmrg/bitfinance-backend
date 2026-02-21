using BitFinance.Business.Enums;

namespace BitFinance.Business.Entities;

public class Expense
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public ExpenseCategory Category { get; set; }
    public decimal Amount { get; set; }
    public ExpenseStatus Status { get; set; }
    public DateTime OccurredAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
    public string CreatedByUserId { get; set; } = null!;
    public User CreatedByUser { get; set; } = null!;
}