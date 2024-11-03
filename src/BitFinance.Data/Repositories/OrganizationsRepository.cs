using System.Linq.Expressions;
using BitFinance.Business.Entities;
using BitFinance.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.Data.Repositories;

public class OrganizationsRepository : IRepository<Organization, Guid>
{
    private readonly ApplicationDbContext _dbContext;

    public OrganizationsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Organization>> GetAllAsync()
    {
        return await _dbContext.Set<Organization>()
            .AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .ToListAsync();
    }
    
    public Task<List<Organization>> GetAllByUserIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Organization?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<Organization>()
            .Where(x => x.Id == id && x.DeletedAt == null)
            .FirstOrDefaultAsync();
    }

    public async Task<Organization> CreateAsync(Organization organization)
    {
        _dbContext.Set<Organization>().Add(organization);
        await _dbContext.SaveChangesAsync();
        return organization;
    }

    public async Task UpdateAsync(Organization organization)
    {
        _dbContext.Set<Organization>().Update(organization);
        await _dbContext.SaveChangesAsync();
    }

    public Task UpdateAsync(Organization entity, params Expression<Func<Organization, object>>[] properties)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Organization organization)
    {
        organization.DeletedAt = DateTime.UtcNow;
        _dbContext.Set<Organization>().Update(organization);
        await _dbContext.SaveChangesAsync();
    }
}