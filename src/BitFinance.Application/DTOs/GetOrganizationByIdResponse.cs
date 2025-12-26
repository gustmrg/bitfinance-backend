namespace BitFinance.Application.DTOs;

public class GetOrganizationByIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public List<UserSummary> Members { get; set; } = [];
}