using BitFinance.Domain.Entities;

namespace BitFinance.Domain.Interfaces;

public interface IBillDocumentRepository
{
    Task<BillDocument?> GetByIdAsync(Guid id);
    Task<BillDocument> CreateAsync(BillDocument document);
    Task UpdateAsync(BillDocument document);
    Task DeleteAsync(Guid id);
}