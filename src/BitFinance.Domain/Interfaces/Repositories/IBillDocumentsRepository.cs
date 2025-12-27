using BitFinance.Domain.Entities;

namespace BitFinance.Domain.Interfaces.Repositories;

public interface IBillDocumentsRepository : IRepository<BillDocument, Guid>
{
    Task<BillDocument?> GetByIdIncludingDeletedAsync(Guid id);
}
