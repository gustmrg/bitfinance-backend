using BitFinance.Business.Entities;

namespace BitFinance.Application.Interfaces;

public interface IOrganizationRepository
{
    Task<Organization?> GetByIdAsync(Guid id);
    Task<List<Organization>> GetByUserIdAsync(Guid userId);
    Task<Organization> CreateAsync(Organization organization);
    Task UpdateAsync(Organization organization);
    Task DeleteAsync(Guid id);
    Task<bool> UserHasAccessAsync(Guid userId, Guid organizationId);
    Task<bool> ExistsAsync(Guid id);
}