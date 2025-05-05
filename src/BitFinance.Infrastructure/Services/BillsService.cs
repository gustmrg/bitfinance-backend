using BitFinance.Domain.Entities;
using BitFinance.Domain.Repositories;
using BitFinance.Domain.Services;

namespace BitFinance.Infrastructure.Services;

public class BillsService : IBillsService
{
    private readonly IBillsRepository _billsRepository;

    public BillsService(IBillsRepository billsRepository)
    {
        _billsRepository = billsRepository;
    }
    
    public async Task<List<Bill>> GetUpcomingBills(Guid organizationId)
    {
        return await _billsRepository.GetUpcomingBills(organizationId);
    }
}