using System.Linq.Expressions;
using BitFinance.Business.Entities;
using BitFinance.Data.Contexts;
using BitFinance.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.Data.Repositories;

public class OrganizationsRepository : IOrganizationsRepository
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
    
    public async Task<List<Organization>> GetAllByUserIdAsync(string userId)
    {
        return await _dbContext.Set<Organization>()
            .AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .Where(x => x.Members.Any(m => m.Id == userId))
            .ToListAsync();
    }

    public async Task<Organization?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<Organization>()
            .Where(x => x.Id == id && x.DeletedAt == null)
            .Include(x => x.Members)
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