using BitFinance.Domain.Entities;

namespace BitFinance.Domain.Interfaces.Repositories;

public interface IOrganizationsRepository : IRepository<Organization, Guid>
{
    Task<List<Organization>> GetAllByUserIdAsync(string userId);
}