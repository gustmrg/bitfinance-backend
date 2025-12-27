using BitFinance.Application.DTOs.Organizations;

namespace BitFinance.Application.DTOs.Common;

public record UserSummary(string Id, string FullName, string Email, string UserName, List<OrganizationSummary>? Organizations = null);
