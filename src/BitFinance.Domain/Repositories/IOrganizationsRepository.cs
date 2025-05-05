using BitFinance.Domain.Entities;

namespace BitFinance.Domain.Repositories;

public interface IOrganizationsRepository : IRepository<Organization, Guid>
{
    Task<List<Organization>> GetAllByUserIdAsync(string userId);
}