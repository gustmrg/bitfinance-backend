using BitFinance.API.Caching;
using BitFinance.Business.Entities;
using BitFinance.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.API.Repositories;

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

    public IEnumerable<Bill> GetAll()
    {
        throw new NotImplementedException();
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
            await _cache.SetAsync(key, bill);
        }

        return bill;
    }

    public async Task DeleteAsync(Bill obj)
    {
        throw new NotImplementedException();
    }

    private bool IsCacheEnabled()
    {
        return _configuration.GetValue<bool>("AppSettings:IsCacheEnabled", defaultValue: false);
    }
}