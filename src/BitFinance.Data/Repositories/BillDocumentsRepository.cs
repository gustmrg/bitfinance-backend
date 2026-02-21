using System.Linq.Expressions;
using BitFinance.Business.Entities;
using BitFinance.Data.Contexts;
using BitFinance.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.Data.Repositories;

public class BillDocumentsRepository : IBillDocumentsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BillDocumentsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<BillDocument>> GetAllAsync()
    {
        return await _dbContext.Set<BillDocument>()
            .AsNoTracking()
            .Where(d => d.DeletedAt == null)
            .OrderByDescending(d => d.UploadedAt)
            .ToListAsync();
    }

    public async Task<BillDocument?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<BillDocument>()
            .FirstOrDefaultAsync(d => d.Id == id && d.DeletedAt == null);
    }

    public async Task<BillDocument> CreateAsync(BillDocument entity)
    {
        _dbContext.Set<BillDocument>().Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(BillDocument entity)
    {
        _dbContext.Set<BillDocument>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public Task UpdateAsync(BillDocument entity, params Expression<Func<BillDocument, object>>[] properties)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(BillDocument entity)
    {
        _dbContext.Set<BillDocument>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
