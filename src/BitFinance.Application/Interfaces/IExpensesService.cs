using BitFinance.Domain.Entities;

namespace BitFinance.Application.Interfaces;

public interface IExpensesService
{
    List<Expense> GetExpensesByOrganization(Guid organizationId);
    Expense GetExpenseById(Guid organizationId, Guid expenseId);
    Task<List<Expense>> GetRecentExpenses(Guid organizationId);
    Guid CreateExpense(Expense expense);
}