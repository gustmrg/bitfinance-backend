using BitFinance.Business.Entities;

namespace BitFinance.Data.Repositories.Interfaces;

public interface IExpensesRepository
{
    Task<List<Expense>> GetAllAsync(Guid organizationId);
    Task<List<Expense>> GetAllByOrganizationAsync(Guid organizationId, int page, int pageSize);
    Task<Expense?> GetByIdAsync(Guid organizationId, Guid expenseId);
    Task<Expense> CreateAsync(Expense expense);
    Task<int> GetEntriesCountAsync();
}