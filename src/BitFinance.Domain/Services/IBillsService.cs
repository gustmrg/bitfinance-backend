using BitFinance.Domain.Entities;

namespace BitFinance.Domain.Services;

public interface IBillsService
{
    Task<List<Bill>> GetUpcomingBills(Guid organizationId);
}