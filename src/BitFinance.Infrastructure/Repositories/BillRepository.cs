using BitFinance.Domain.Entities;
using BitFinance.Domain.Enums;
using BitFinance.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.Infrastructure.Repositories;

public class BillRepository : IBillRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BillRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Bill?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Bills.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Bill>> GetAllByOrganizationAsync(
        Guid organizationId, 
        int page, int pageSize, 
        DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _dbContext.Bills
            .AsNoTracking()
            .Where(b => b.DeletedAt == null)
            .Where(b => b.OrganizationId == organizationId);

        if (startDate.HasValue)
        {
            query = query.Where(b => b.DueDate >= startDate);
        }

        if (endDate.HasValue)
        {
            query = query.Where(b => b.DueDate <= endDate);
        }
        
        return await query
            .Include(b => b.Documents)
            .OrderBy(b => b.DueDate)
            .Skip(pageSize * (page - 1))
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Bill>> GetAllByStatusAsync(BillStatus status)
    {
        return await _dbContext.Bills
            .AsNoTracking()
            .Where(b => b.DeletedAt == null && b.Status == status)
            .OrderBy(b => b.DueDate)
            .ToListAsync();
    }

    public async Task<Bill> CreateAsync(Bill bill)
    {
        _dbContext.Bills.Add(bill);
        await _dbContext.SaveChangesAsync();
        return bill;
    }

    public async Task UpdateAsync(Bill bill)
    {
        _dbContext.Bills.Update(bill);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        _dbContext.Bills.Remove(await _dbContext.Bills.FindAsync(id));
        await _dbContext.SaveChangesAsync();
    }
}