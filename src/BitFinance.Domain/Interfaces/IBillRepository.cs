using BitFinance.Domain.Entities;
using BitFinance.Domain.Enums;

namespace BitFinance.Domain.Interfaces;

public interface IBillRepository
{
    Task<Bill?> GetByIdAsync(Guid id);
    Task<List<Bill>> GetAllByOrganizationAsync(Guid organizationId, int page, int pageSize, DateTime? startDate = null, DateTime? endDate = null);
    Task<List<Bill>> GetAllByStatusAsync(BillStatus status);
    Task<Bill> CreateAsync(Bill bill);
    Task UpdateAsync(Bill bill);
    Task DeleteAsync(Guid id);
}