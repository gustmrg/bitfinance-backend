using BitFinance.Application.DTOs.Common;

namespace BitFinance.Application.DTOs.Organizations;

public class GetOrganizationByIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public List<UserSummary> Members { get; set; } = [];
}
