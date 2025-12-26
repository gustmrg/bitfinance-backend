namespace BitFinance.Application.DTOs;

public record GetMeResponse(string Id, string Username, string Email, List<OrganizationSummary> Organizations);