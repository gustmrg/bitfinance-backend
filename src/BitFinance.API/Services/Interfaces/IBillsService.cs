using BitFinance.Business.Entities;

namespace BitFinance.API.Services.Interfaces;

public interface IBillsService
{
    Task<List<Bill>> GetUpcomingBills(Guid organizationId);
}