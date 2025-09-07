using BitFinance.Domain.Entities;
using BitFinance.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ExpenseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Expense>> GetAllAsync(Guid organizationId)
    {
        return await _dbContext.Expenses
            .AsNoTracking()
            .Where(x => x.OrganizationId == organizationId)
            .ToListAsync();
    }

    public async Task<List<Expense>> GetAllByOrganizationAsync(Guid organizationId, 
        int page, int pageSize, 
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var query = _dbContext.Expenses
            .AsNoTracking()
            .Where(b => b.OrganizationId == organizationId);

        if (startDate.HasValue)
        {
            query = query.Where(b => b.CreatedAt >= startDate);
        }

        if (endDate.HasValue)
        {
            query = query.Where(b => b.CreatedAt <= endDate);
        }
        
        return await query
            .Include(e => e.CreatedByUser)
            .OrderBy(e => e.CreatedAt)
            .Skip(pageSize * (page - 1))
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Expense>> GetRecentExpenses(Guid organizationId)
    {
        return await _dbContext.Expenses
            .AsNoTracking()
            .Where(e => e.OrganizationId == organizationId && 
                        e.OccurredAt.Month == DateTime.Now.Month)
            .OrderByDescending(e => e.OccurredAt)
            .ToListAsync();
    }

    public async Task<Expense?> GetByIdAsync(Guid expenseId)
    {
        return await _dbContext.Expenses
            .Where(e => e.Id == expenseId)
            .FirstOrDefaultAsync();
    }

    public async Task<Expense> CreateAsync(Expense expense)
    {
        _dbContext.Expenses.Add(expense);
        await _dbContext.SaveChangesAsync();
        return expense;
    }

    public async Task<Expense> UpdateAsync(Expense expense)
    {
        _dbContext.Expenses.Update(expense);
        await _dbContext.SaveChangesAsync();
        return expense;
    }

    public async Task DeleteAsync(Guid expenseId)
    {
        _dbContext.Bills.Remove(await _dbContext.Bills.FindAsync(expenseId));
        await _dbContext.SaveChangesAsync();
    }
}