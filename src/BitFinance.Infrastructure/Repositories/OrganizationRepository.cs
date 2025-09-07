using BitFinance.Domain.Entities;
using BitFinance.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.Infrastructure.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrganizationRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Organization?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Organizations
            .Where(x => x.Id == id && x.DeletedAt == null)
            .Include(x => x.Members)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Organization>> GetByUserIdAsync(string userId)
    {
        return await _dbContext.Organizations
            .AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .Where(x => x.Members.Any(m => m.Id == userId))
            .ToListAsync();
    }

    public async Task<Organization> CreateAsync(Organization organization)
    {
        _dbContext.Organizations.Add(organization);
        await _dbContext.SaveChangesAsync();
        return organization;
    }

    public async Task UpdateAsync(Organization organization)
    {
        _dbContext.Organizations.Update(organization);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        _dbContext.Organizations.Remove(await _dbContext.Organizations.FindAsync(id));
        await _dbContext.SaveChangesAsync();
    }
}