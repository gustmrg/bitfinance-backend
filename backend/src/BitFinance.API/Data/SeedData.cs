using BitFinance.Business.Entities;
using BitFinance.Business.Enums;
using BitFinance.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.API.Data;

public static class SeedData
{
    public static void SeedDatabase(WebApplication app)
    {
        var isDatabaseSeedEnabled = app.Configuration.GetValue<bool>("AppSettings:DatabaseSeedEnabled");

        if (!isDatabaseSeedEnabled)
        {
            Console.WriteLine("Database seeding is disabled. Skipping...");
            return; 
        }
    
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
    
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            context.Database.EnsureCreated();
            Initialize(services);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred seeding the DB: {ExceptionMessage}", ex.Message);
            Console.WriteLine("An error has occurred and could not seed database");
        }
    }
    
    private static void Initialize(IServiceProvider serviceProvider)
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
            Name = "Internet Bill",
            Category = BillCategory.Utilities,
            Status = BillStatus.Overdue,
            CreatedDate = DateTime.UtcNow.AddHours(-3),
            DueDate = DateTime.UtcNow.AddHours(-3),
            PaidDate = null,
            AmountDue = 120M,
            AmountPaid = null
        };
        
        Bill bill2 = new()
        {
            Name = "Electricity Bill",
            Category = BillCategory.Utilities,
            Status = BillStatus.Paid,
            CreatedDate = DateTime.UtcNow.AddHours(-3),
            DueDate = DateTime.UtcNow.AddHours(-3),
            PaidDate = DateTime.Now.ToUniversalTime().AddHours(-3),
            AmountDue = 800M,
            AmountPaid = 800M,
            IsPaid = true
        };
        
        Bill bill3 = new()
        {
            Name = "Health Insurance",
            Category = BillCategory.Healthcare,
            Status = BillStatus.Paid,
            CreatedDate = DateTime.UtcNow.AddHours(-3),
            DueDate = DateTime.UtcNow.AddHours(-3),
            PaidDate = DateTime.UtcNow.AddHours(-3).AddDays(-2),
            AmountDue = 500M,
            AmountPaid = 500M,
            IsPaid = true
        };
        
        Bill bill4 = new()
        {
            Name = "Car Insurance",
            Category = BillCategory.Insurance,
            Status = BillStatus.Due,
            CreatedDate = DateTime.UtcNow.AddHours(-3),
            DueDate = DateTime.UtcNow.AddHours(-3).AddDays(10),
            AmountDue = 1000M,
            IsPaid = false
        };
        
        Bill bill5 = new()
        {
            Name = "Streaming Subscription",
            Category = BillCategory.Entertainment,
            Status = BillStatus.Due,
            CreatedDate = DateTime.UtcNow.AddHours(-3),
            DueDate = DateTime.UtcNow.AddHours(-3).AddDays(15),
            AmountDue = 49.90M,
            IsPaid = false,
        };

        Bill bill6 = new() 
        {
            Name = "Rent Payment",
            Category = BillCategory.Housing,
            Status = BillStatus.Paid,
            CreatedDate = DateTime.UtcNow.AddHours(-3),
            DueDate = DateTime.UtcNow.AddHours(-3).AddDays(-2),
            PaidDate = DateTime.UtcNow.AddHours(-3),
            AmountDue = 2000M,
            AmountPaid = 2000M,
            IsPaid = true
        };

        Bill bill7 = new()
        {
            Name = "Online Course Subscription",
            Category = BillCategory.Education,
            Status = BillStatus.Cancelled,
            CreatedDate = DateTime.UtcNow.AddHours(-3),
            DueDate = DateTime.UtcNow.AddHours(-3).AddDays(20),
            AmountDue = 250M,
            IsPaid = false
        };
        
        dbContext.AddRange(bill1, bill2, bill3, bill4, bill5, bill6, bill7);

        dbContext.SaveChanges();
    }
}