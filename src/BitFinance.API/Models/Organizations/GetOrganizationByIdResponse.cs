using BitFinance.API.Models.Users;

namespace BitFinance.API.Models.Organizations;

public class GetOrganizationByIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public List<MemberUserResponse> Members { get; set; } = [];
}