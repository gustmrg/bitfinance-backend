using System.Linq.Expressions;
using BitFinance.Domain.Entities;
using BitFinance.Domain.Interfaces.Repositories;
using BitFinance.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.Infrastructure.Persistence.Repositories;

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
            .ToListAsync();
    }

    public async Task<BillDocument?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<BillDocument>()
            .FirstOrDefaultAsync(d => d.Id == id && d.DeletedAt == null);
    }

    public async Task<BillDocument?> GetByIdIncludingDeletedAsync(Guid id)
    {
        return await _dbContext.Set<BillDocument>()
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<BillDocument> CreateAsync(BillDocument document)
    {
        _dbContext.Set<BillDocument>().Add(document);
        await _dbContext.SaveChangesAsync();
        return document;
    }

    public async Task UpdateAsync(BillDocument document)
    {
        _dbContext.Set<BillDocument>().Update(document);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(BillDocument document, params Expression<Func<BillDocument, object>>[] properties)
    {
        _dbContext.Attach(document);
        var entry = _dbContext.Entry(document);
        foreach (var property in properties)
        {
            entry.Property(property).IsModified = true;
        }
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(BillDocument document)
    {
        document.DeletedAt = DateTime.UtcNow;
        _dbContext.Set<BillDocument>().Update(document);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
