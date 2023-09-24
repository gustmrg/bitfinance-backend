using BitFinance.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Bill> Bills => Set<Bill>();
}