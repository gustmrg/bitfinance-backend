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
    
    Task<Result<List<BillDto>>> GetAllByOrganizationAsync(
        Guid organizationId,
        int page, int pageSize = 100, 
        DateTime? from = null, DateTime? to = null,
        CancellationToken cancellationToken = default);
    
    Task UpdateAsync(Bill bill);
}