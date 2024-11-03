namespace BitFinance.Business.Entities;

public class Organization
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public ICollection<User> Members { get; set; } = new List<User>();
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
}