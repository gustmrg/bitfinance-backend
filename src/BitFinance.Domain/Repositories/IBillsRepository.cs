using BitFinance.Domain.Entities;
using BitFinance.Domain.Enums;

namespace BitFinance.Domain.Repositories;

public interface IBillsRepository : IRepository<Bill, Guid>
{
    Task<List<Bill>> GetAllByOrganizationAsync(Guid organizationId, int page, int pageSize, DateTime? startDate = null, DateTime? endDate = null);
    Task<List<Bill>> GetAllByStatusAsync(BillStatus billStatus);
    Task<int> GetEntriesCountAsync();
    Task<List<Bill>> GetUpcomingBills(Guid organizationId);
}