using BitFinance.Business.Entities;

namespace BitFinance.Data.Repositories.Interfaces;

public interface IExpensesRepository: IRepository<Expense, Guid>
{
    Task<List<Expense>> GetAllByOrganizationAsync(Guid organizationId);
}