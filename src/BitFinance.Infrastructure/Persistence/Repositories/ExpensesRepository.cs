using BitFinance.Domain.Entities;
using BitFinance.Domain.Interfaces.Repositories;
using BitFinance.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.Infrastructure.Persistence.Repositories;

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
    
    public async Task<List<Expense>> GetAllByOrganizationAsync(Guid organizationId, int page, int pageSize, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _dbContext.Set<Expense>()
            .AsNoTracking()
            .Where(b => b.DeletedAt == null)
            .Where(b => b.OrganizationId == organizationId);

        if (startDate.HasValue)
        {
            query = query.Where(e => e.CreatedAt >= startDate);
        }

        if (endDate.HasValue)
        {
            query = query.Where(e => e.CreatedAt <= endDate);
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
        return await _dbContext.Set<Expense>()
            .AsNoTracking()
            .Where(b => b.OrganizationId == organizationId && b.DeletedAt == null)
            .OrderByDescending(e => e.OccurredAt)
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