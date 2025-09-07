using BitFinance.Domain.Entities;
using BitFinance.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.Infrastructure.Repositories;

public class BillDocumentRepository : IBillDocumentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BillDocumentRepository(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public async Task<BillDocument?> GetByIdAsync(Guid id)
    {
        return await _dbContext.BillDocuments.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<BillDocument> CreateAsync(BillDocument document)
    {
        _dbContext.BillDocuments.Add(document);
        await _dbContext.SaveChangesAsync();
        return document;
    }

    public async Task UpdateAsync(BillDocument document)
    {
        _dbContext.BillDocuments.Update(document);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}