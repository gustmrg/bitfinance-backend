using BitFinance.Data.Caching;
using BitFinance.Data.Contexts;
using BitFinance.Data.Repositories;
using BitFinance.Data.Repositories.Interfaces;
using BitFinance.WorkerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

var builder = Host.CreateApplicationBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Database");

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseNpgsql(connectionString));

builder.Services.AddSingleton<ICacheService, RedisCacheService>();
builder.Services.AddSingleton<DistributedCacheEntryOptions>();
builder.Services.AddScoped<IBillsRepository, BillsRepository>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Cache");
    options.InstanceName = "BitFinance";
});
builder.Services.AddHostedService<Worker>();


var host = builder.Build();
host.Run();