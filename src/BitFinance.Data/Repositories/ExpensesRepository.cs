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

    public async Task<List<Expense>> GetAllAsync()
    {
        return await _dbContext.Set<Expense>()
            .AsNoTracking()
            .Where(b => b.DeletedAt == null)
            .ToListAsync();
    }
    
    public async Task<List<Expense>> GetAllByOrganizationAsync(Guid organizationId)
    {
        return await _dbContext.Set<Expense>()
            .AsNoTracking()
            .Where(b => b.DeletedAt == null)
            .Where(b => b.OrganizationId == organizationId)
            .ToListAsync();
    }

    public async Task<Expense?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<Expense>().FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
    }

    public async Task<Expense> CreateAsync(Expense entity)
    {
        _dbContext.Set<Expense>().Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Expense entity)
    {
        _dbContext.Set<Expense>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Expense entity, params Expression<Func<Expense, object>>[] properties)
    {
        _dbContext.Attach(entity);
        
        var entry = _dbContext.Entry(entity);

        foreach (var property in properties)
        {
            entry.Property(property).IsModified = true;
        }
        
        entity.UpdatedAt = DateTime.UtcNow;
        entry.Property(x => x.UpdatedAt).IsModified = true;
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Expense entity)
    {
        entity.DeletedAt = DateTime.UtcNow;
        _dbContext.Set<Expense>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }
}