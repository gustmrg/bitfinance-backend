using BitFinance.Application.Common;
using BitFinance.Application.DTOs.Bills;
using BitFinance.Domain.Entities;
using BitFinance.Domain.Enums;

namespace BitFinance.Application.Interfaces;

public interface IBillService
{
    Task<Result<BillDto>> CreateAsync(Guid organizationId, CreateBillRequestDto request,
        CancellationToken cancellationToken = default);
    Task<Result<BillDto>> GetByIdAsync(Guid id);
    Task<List<Bill>> GetAllBillsByStatusAsync(BillStatus status);
    Task UpdateAsync(Bill bill);
}