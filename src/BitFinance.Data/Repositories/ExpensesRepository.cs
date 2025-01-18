using System.Linq.Expressions;
using BitFinance.Business.Entities;
using BitFinance.Data.Contexts;
using BitFinance.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.Data.Repositories;

public class ExpensesRepository : IExpensesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ExpensesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Expense>> GetAllAsync(Guid organizationId)
    {
        return await _dbContext.Set<Expense>()
            .AsNoTracking()
            .Where(b => b.OrganizationId == organizationId)
            .Where(b => b.DeletedAt == null)
            .ToListAsync();
    }
    
    public async Task<List<Expense>> GetAllByOrganizationAsync(Guid organizationId, int page, int pageSize)
    {
        return await _dbContext.Set<Expense>()
            .AsNoTracking()
            .Where(b => b.DeletedAt == null)
            .Where(b => b.OrganizationId == organizationId)
            .OrderBy(b => b.CreatedAt)
            .Skip(pageSize * (page - 1))
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Expense?> GetByIdAsync(Guid expenseId)
    {
        return await _dbContext.Set<Expense>()
            .Where(e => e.Id == expenseId)
            .FirstOrDefaultAsync();
    }

    public async Task<Expense> CreateAsync(Expense expense)
    {
        _dbContext.Set<Expense>().Add(expense);
        await _dbContext.SaveChangesAsync();
        return expense;
    }

    public async Task<Expense> UpdateAsync(Expense expense)
    {
        _dbContext.Set<Expense>().Update(expense);
        await _dbContext.SaveChangesAsync();
        return expense;
    }

    public async Task DeleteAsync(Expense expense)
    {
        expense.DeletedAt = DateTime.UtcNow;
        _dbContext.Set<Expense>().Update(expense);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> GetEntriesCountAsync()
    {
        return await _dbContext.Expenses.CountAsync();
    }
}