using BitFinance.Domain.Entities;
using BitFinance.Domain.Enums;

namespace BitFinance.Application.Interfaces;

public interface IBillService
{
    Task<Bill?> GetByIdAsync(Guid id);
    Task<List<Bill>> GetAllBillsByStatusAsync(BillStatus status);
    Task UpdateAsync(Bill bill);
}