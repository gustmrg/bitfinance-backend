using BitFinance.API.Caching;
using BitFinance.Business.Entities;
using BitFinance.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.API.Repositories;

public class BillsRepository : IRepository<Bill>
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

    public async Task<Bill?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        Bill? bill;
        
        if (IsCacheEnabled())
        {
            string key = _cache.GenerateKey<Bill>(id.ToString());
            bill = await _cache.GetAsync<Bill>(key, cancellationToken);

            if (bill is null)
            {
                bill = await _dbContext.Bills.FirstOrDefaultAsync(x => x.Id == id && x.DeletedDate == null, 
                    cancellationToken);

                if (bill is not null)
                {
                    await _cache.SetAsync(key, bill, cancellationToken);
                }
            }
        }
        else
        {
            bill = await _dbContext.Bills.FirstOrDefaultAsync(x => x.Id == id && x.DeletedDate == null,
                cancellationToken);
        }
        
        return bill;
    }

    public Bill Add()
    {
        throw new NotImplementedException();
    }

    public bool IsCacheEnabled()
    {
        return _configuration.GetValue<bool>("AppSettings:IsCacheEnabled", defaultValue: false);
    }
}