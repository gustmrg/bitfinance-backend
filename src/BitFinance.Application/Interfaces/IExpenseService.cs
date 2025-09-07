using BitFinance.Domain.Entities;

namespace BitFinance.Application.Interfaces;

public interface IExpenseService
{
    Task<List<Expense>> GetExpensesByOrganization(Guid organizationId);
    Task<Expense?> GetExpenseById(Guid organizationId, Guid expenseId);
    Task<List<Expense>> GetRecentExpenses(Guid organizationId);
    Task<Expense> CreateExpense(Expense expense);
}