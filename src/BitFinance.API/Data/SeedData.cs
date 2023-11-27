using BitFinance.Business.Enums;
using BitFinance.Business.Models;
using BitFinance.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.API.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var dbContext = new ApplicationDbContext(
                   serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            // Look for any Bills.
            if (dbContext.Bills.Any())
            {
                return;   // DB has been seeded
            }

            PopulateTestData(dbContext);
        }
    }
    
    private static void PopulateTestData(ApplicationDbContext dbContext)
    {
        foreach (var item in dbContext.Bills)
        {
            dbContext.Remove(item);
        }
        dbContext.SaveChanges();

        Bill bill1 = new()
        {
            Name = "Vivo Fibra",
            Category = BillCategory.Utilities,
            CreatedDate = DateTime.UtcNow.AddHours(-3),
            DueDate = DateTime.UtcNow.AddHours(-3),
            AmountDue = 120M,
            IsPaid = false
        };
        
        Bill bill2 = new()
        {
            Name = "Cagece",
            Category = BillCategory.Utilities,
            CreatedDate = DateTime.UtcNow.AddHours(-3),
            DueDate = DateTime.UtcNow.AddHours(-3),
            PaidDate = DateTime.Now.ToUniversalTime().AddHours(-3),
            AmountDue = 80M,
            AmountPaid = 80M,
            IsPaid = true
        };
        
        Bill bill3 = new()
        {
            Name = "Unimed CE",
            Category = BillCategory.Healthcare,
            CreatedDate = DateTime.UtcNow.AddHours(-3),
            DueDate = DateTime.UtcNow.AddHours(-3),
            PaidDate = new DateTime(2023, 11, 18).ToUniversalTime().AddHours(-3),
            AmountDue = 350M,
            AmountPaid = 350M,
            IsPaid = true
        };
        
        Bill bill4 = new()
        {
            Name = "Allianz Seguros",
            Category = BillCategory.Insurance,
            CreatedDate = DateTime.UtcNow.AddHours(-3),
            DueDate = new DateTime(2023, 12, 10).ToUniversalTime().AddHours(-3),
            AmountDue = 1000M,
            IsPaid = false
        };
        
        Bill bill5 = new()
        {
            Name = "Netflix",
            Category = BillCategory.Entertainment,
            CreatedDate = DateTime.UtcNow.AddHours(-3),
            DueDate = new DateTime(2023, 12, 01).ToUniversalTime().AddHours(-3),
            AmountDue = 49.90M,
            IsPaid = false,
            IsDeleted = true
        };

        dbContext.Bills.Add(bill1);
        dbContext.Bills.Add(bill2);
        dbContext.Bills.Add(bill3);
        dbContext.Bills.Add(bill4);
        dbContext.Bills.Add(bill5);

        dbContext.SaveChanges();
    }
}