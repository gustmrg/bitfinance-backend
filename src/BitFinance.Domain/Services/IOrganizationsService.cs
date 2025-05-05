namespace BitFinance.Domain.Services;

public interface IOrganizationsService
{
    Task<bool> ValidateOrganizationExistsAsync(Guid organizationId);
    Task<bool> ValidateUserBelongsToOrganizationAsync(string userId, Guid organizationId);
}