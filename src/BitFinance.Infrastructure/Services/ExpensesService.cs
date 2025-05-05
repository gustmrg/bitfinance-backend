using BitFinance.Domain.Entities;
using BitFinance.Domain.Repositories;
using BitFinance.Domain.Services;

namespace BitFinance.Infrastructure.Services;

public class ExpensesService : IExpensesService
{
    private readonly IExpensesRepository _expensesRepository;

    public ExpensesService(IExpensesRepository expensesRepository)
    {
        _expensesRepository = expensesRepository;
    }
    
    public List<Expense> GetExpensesByOrganization(Guid organizationId)
    {
        throw new NotImplementedException();
    }

    public Expense GetExpenseById(Guid organizationId, Guid expenseId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Expense>> GetRecentExpenses(Guid organizationId)
    {
        return await _expensesRepository.GetRecentExpenses(organizationId);
    }

    public Guid CreateExpense(Expense expense)
    {
        throw new NotImplementedException();
    }
}