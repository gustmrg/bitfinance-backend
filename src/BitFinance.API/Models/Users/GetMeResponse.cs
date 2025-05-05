using BitFinance.API.Models.Organizations;

namespace BitFinance.API.Models.Users;

public record GetMeResponse(string Id, string Username, string Email, List<OrganizationResponse> Organizations);