using BitFinance.API.Repositories;
using BitFinance.Business.Entities;
using BitFinance.Data.Caching;
using BitFinance.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BitFinance.Data.Repositories;

public class BillsRepository : IRepository<Bill, Guid>
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public BillsRepository(IConfiguration configuration, ApplicationDbContext dbContext, ICacheService cache)
    {
        _configuration = configuration;
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<List<Bill>> GetAll()
    {
        List<Bill> list = await _dbContext.Set<Bill>().AsNoTracking()
            .Where(b => b.DeletedDate == null)
            .ToListAsync();

        return list;
    }

    public async Task<Bill?> GetByIdAsync(Guid id)
    {
        Bill? bill;
        
        if (IsCacheEnabled())
        {
            string key = _cache.GenerateKey<Bill>(id.ToString());
            bill = await _cache.GetAsync<Bill>(key);

            if (bill is null)
            {
                bill = await _dbContext.Set<Bill>().FirstOrDefaultAsync(x => x.Id == id && x.DeletedDate == null);

                if (bill is not null)
                {
                    await _cache.SetAsync(key, bill);
                }
            }
        }
        else
        {
            bill = await _dbContext.Set<Bill>().FirstOrDefaultAsync(x => x.Id == id && x.DeletedDate == null);
        }
        
        return bill;
    }

    public async Task<Bill> CreateAsync(Bill bill)
    {
        _dbContext.Set<Bill>().Add(bill);
        await _dbContext.SaveChangesAsync();

        if (IsCacheEnabled())
        {
            string key = _cache.GenerateKey<Bill>(bill.Id.ToString());
            await _cache.SetAsync(key, bill, TimeSpan.FromHours(1));
        }

        return bill;
    }

    public async Task UpdateAsync(Bill bill)
    {
        _dbContext.Set<Bill>().Update(bill);
        await _dbContext.SaveChangesAsync();
        
        if (IsCacheEnabled())
        {
            string key = _cache.GenerateKey<Bill>(bill.Id.ToString());
            await _cache.SetAsync(key, bill);
        }
    }

    public async Task DeleteAsync(Bill bill)
    {
        bill.DeletedDate = DateTime.UtcNow;
        _dbContext.Set<Bill>().Update(bill);
        await _dbContext.SaveChangesAsync();
        
        if (IsCacheEnabled())
        {
            string key = _cache.GenerateKey<Bill>(bill.Id.ToString());
            await _cache.SetAsync(key, bill);
        }
    }

    private bool IsCacheEnabled()
    {
        return Convert.ToBoolean(_configuration.GetSection("AppSettings:IsCacheEnabled").Value);
    }
}