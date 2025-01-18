using BitFinance.Business.Entities;

namespace BitFinance.API.Services.Interfaces;

public interface IExpensesService
{
    List<Expense> GetExpensesByOrganization(Guid organizationId);
    Expense GetExpenseById(Guid organizationId, Guid expenseId);
    Guid CreateExpense(Expense expense);
}