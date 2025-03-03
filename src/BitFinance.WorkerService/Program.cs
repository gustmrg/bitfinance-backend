using BitFinance.Data.Caching;
using BitFinance.Data.Contexts;
using BitFinance.Data.Repositories;
using BitFinance.Data.Repositories.Interfaces;
using BitFinance.WorkerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<IBillsRepository, BillsRepository>();

builder.Services.AddSingleton<ICacheService, RedisCacheService>();
builder.Services.AddSingleton<DistributedCacheEntryOptions>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Cache");
    options.InstanceName = "BitFinance";
});

var host = builder.Build();
host.Run();