using BitFinance.Domain.Entities;

namespace BitFinance.Application.Interfaces;

public interface IOrganizationService
{
    Task<Organization?> GetByIdAsync(Guid id);
    Task<List<Organization>> GetByUserIdAsync(string userId);
    Task<Organization> CreateAsync(Organization organization);
    Task UpdateAsync(Organization organization);
    Task DeleteAsync(Guid id);
    Task<bool> UserHasAccessAsync(string userId, Guid organizationId);
    Task<bool> ExistsAsync(Guid id);
}