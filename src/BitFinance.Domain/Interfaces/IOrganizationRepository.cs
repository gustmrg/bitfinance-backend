using BitFinance.Domain.Entities;

namespace BitFinance.Domain.Interfaces;

public interface IOrganizationRepository
{
    Task<Organization?> GetByIdAsync(Guid id);
    Task<List<Organization>> GetByUserIdAsync(string userId);
    Task<Organization> CreateAsync(Organization organization);
    Task UpdateAsync(Organization organization);
    Task DeleteAsync(Guid id);
}