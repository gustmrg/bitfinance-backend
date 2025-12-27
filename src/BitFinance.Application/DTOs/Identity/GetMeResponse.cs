using BitFinance.Application.DTOs.Organizations;

namespace BitFinance.Application.DTOs.Identity;

public record GetMeResponse(string Id, string Username, string Email, List<OrganizationSummary> Organizations);
