using BitFinance.Application.Interfaces;
using BitFinance.Domain.Entities;
using BitFinance.Domain.Enums;
using BitFinance.Domain.Interfaces;

namespace BitFinance.Application.Services;

public class BillService : IBillService
{
    private readonly IBillRepository _repository;

    public BillService(IBillRepository repository)
    {
        _repository = repository;
    }

    public async Task<Bill?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<List<Bill>> GetAllBillsByStatusAsync(BillStatus status)
    {
        return await _repository.GetAllByStatusAsync(status);
    }

    public async Task UpdateAsync(Bill bill)
    {
        await _repository.UpdateAsync(bill);
    }
}