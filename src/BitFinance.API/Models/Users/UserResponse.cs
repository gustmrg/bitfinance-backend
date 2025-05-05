using BitFinance.API.Models.Organizations;

namespace BitFinance.API.Models.Users;

public record UserResponse(string Id, string FullName, string Email, string UserName, List<OrganizationResponse> Organizations);