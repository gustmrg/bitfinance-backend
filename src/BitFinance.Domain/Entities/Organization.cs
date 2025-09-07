namespace BitFinance.Domain.Entities;

public class Organization
{
    public Organization(string name)
    {
        Name = name;
    }
    
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<User> Members { get; set; } = new List<User>();
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}