using BitFinance.Domain.Entities;

namespace BitFinance.Domain.Interfaces;

public interface IExpenseRepository
{
    Task<List<Expense>> GetAllAsync(Guid organizationId);
    Task<List<Expense>> GetAllByOrganizationAsync(Guid organizationId, int page, int pageSize, DateTime? startDate = null, DateTime? endDate = null);
    Task<List<Expense>> GetRecentExpenses(Guid organizationId);
    Task<Expense?> GetByIdAsync(Guid expenseId);
    Task<Expense> CreateAsync(Expense expense);
    Task<Expense> UpdateAsync(Expense expense);
    Task DeleteAsync(Guid expenseId);
}