using BitFinance.Application.Interfaces;
using BitFinance.Domain.Entities;
using BitFinance.Domain.Interfaces;

namespace BitFinance.Application.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expensesRepository;

    public ExpenseService(IExpenseRepository expensesRepository)
    {
        _expensesRepository = expensesRepository;
    }

    public async Task<List<Expense>> GetExpensesByOrganization(Guid organizationId)
    {
        throw new NotImplementedException();
    }

    public async Task<Expense?> GetExpenseById(Guid organizationId, Guid expenseId)
    {
        return await _expensesRepository.GetByIdAsync(expenseId);
    }

    public async Task<List<Expense>> GetRecentExpenses(Guid organizationId)
    {
        throw new NotImplementedException();
    }

    public async Task<Expense> CreateExpense(Expense expense)
    {
        return await _expensesRepository.CreateAsync(expense);
    }
}