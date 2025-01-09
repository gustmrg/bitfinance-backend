using BitFinance.Business.Entities;

namespace BitFinance.Data.Repositories.Interfaces;

public interface IExpensesRepository
{
    Task<List<Expense>> GetAllAsync(Guid organizationId);
    Task<Expense?> GetByIdAsync(Guid organizationId, Guid expenseId);
    Task<Expense> CreateAsync(Expense expense);
}