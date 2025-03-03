using System.Linq.Expressions;
using BitFinance.Business.Entities;
using BitFinance.Data.Contexts;
using BitFinance.Data.Repositories.Interfaces;

namespace BitFinance.Data.Repositories;

public class FileRecordsRepository : IFileRecordsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FileRecordsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<FileRecord>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<FileRecord?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<FileRecord> CreateAsync(FileRecord entity)
    {
        _dbContext.Set<FileRecord>().Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(FileRecord entity)
    {
        _dbContext.Set<FileRecord>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public Task UpdateAsync(FileRecord entity, params Expression<Func<FileRecord, object>>[] properties)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(FileRecord entity)
    {
        throw new NotImplementedException();
    }
}