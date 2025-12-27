using BitFinance.Application.Interfaces;
using BitFinance.Domain.Entities;
using BitFinance.Domain.Interfaces.Repositories;

namespace BitFinance.Application.Services;

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