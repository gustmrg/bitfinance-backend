using BitFinance.Domain.Enums;
using BitFinance.Domain.ValueObjects;

namespace BitFinance.Domain.Entities;

public class Expense
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public ExpenseCategory Category { get; set; }
    public Money Amount { get; set; } = null!;
    public ExpenseStatus Status { get; set; }
    public DateTime OccurredAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
    public string CreatedByUserId { get; set; } = null!;
    public User CreatedByUser { get; set; } = null!;
}