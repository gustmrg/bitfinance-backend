using BitFinance.Business.Entities;

namespace BitFinance.Data.Repositories.Interfaces;

public interface IBillsRepository : IRepository<Bill, Guid>
{
    Task<List<Bill>> GetAllByOrganizationAsync(Guid organizationId, int page, int pageSize,
        DateTime? startDate = null, DateTime? endDate = null);
    Task<int> GetEntriesCountAsync();
}