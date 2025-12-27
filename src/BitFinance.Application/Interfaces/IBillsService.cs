using BitFinance.Domain.Entities;

namespace BitFinance.Application.Interfaces;

public interface IBillsService
{
    Task<List<Bill>> GetUpcomingBills(Guid organizationId);
}