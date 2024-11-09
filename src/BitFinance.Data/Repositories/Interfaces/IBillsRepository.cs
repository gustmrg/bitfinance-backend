using BitFinance.Business.Entities;

namespace BitFinance.Data.Repositories.Interfaces;

public interface IBillsRepository : IRepository<Bill, Guid>
{
    Task<List<Bill>> GetAllByOrganizationAsync(Guid organizationId);
}