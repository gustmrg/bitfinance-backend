using BitFinance.Business.Entities;

namespace BitFinance.Data.Repositories.Interfaces;

public interface IBillDocumentsRepository : IRepository<BillDocument, Guid>
{
    Task<long> GetTotalStorageByOrganizationAsync(Guid organizationId);
}
