namespace BitFinance.Application.DTOs;

public record UserSummary(string Id, string FullName, string Email, string UserName, List<OrganizationSummary>? Organizations = null);
