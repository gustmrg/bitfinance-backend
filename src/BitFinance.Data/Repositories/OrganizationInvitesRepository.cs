using System.Linq.Expressions;
using BitFinance.Business.Entities;
using BitFinance.Data.Contexts;
using BitFinance.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.Data.Repositories;

public class OrganizationInvitesRepository : IOrganizationInvitesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrganizationInvitesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<OrganizationInvite>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<OrganizationInvite?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<OrganizationInvite>()
            .Where(x=> x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<OrganizationInvite> CreateAsync(OrganizationInvite entity)
    {
        _dbContext.Set<OrganizationInvite>().Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(OrganizationInvite entity)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(OrganizationInvite entity, params Expression<Func<OrganizationInvite, object>>[] properties)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(OrganizationInvite entity)
    {
        throw new NotImplementedException();
    }
    
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}