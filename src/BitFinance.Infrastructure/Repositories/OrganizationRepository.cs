using BitFinance.Application.Interfaces;
using BitFinance.Business.Entities;

namespace BitFinance.Infrastructure.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    public async Task<Organization?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Organization>> GetByUserIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Organization> CreateAsync(Organization organization)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(Organization organization)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UserHasAccessAsync(Guid userId, Guid organizationId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}