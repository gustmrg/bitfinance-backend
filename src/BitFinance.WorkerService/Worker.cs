using BitFinance.Business.Enums;
using BitFinance.Data.Repositories.Interfaces;

namespace BitFinance.WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private IServiceScopeFactory _serviceScopeFactory;

    public Worker(ILogger<Worker> logger, 
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await UpdateUpcomingBills();
            await UpdateDueBills();
            
            var now = DateTime.UtcNow;
            var nextHour = now.Date.AddHours(now.Hour + 1);
            var delay = nextHour - now;

            await Task.Delay(delay, stoppingToken);
        }
    }

    private async Task UpdateUpcomingBills()
    {
        using (IServiceScope scope = _serviceScopeFactory.CreateScope())
        {
            IBillsRepository billsRepository = scope.ServiceProvider.GetRequiredService<IBillsRepository>();

            try
            {
                var upcomingBills = await billsRepository.GetAllByStatusAsync(BillStatus.Upcoming);
                var today = DateTime.UtcNow.Date;
                var billsUpdated = 0;

                foreach (var bill in upcomingBills)
                {
                    if (bill.DueDate.Date == today)
                    {
                        bill.Status = BillStatus.Due;
                        bill.UpdatedAt = today;
                        billsUpdated++;
                    }
                    else if (bill.DueDate.Date < today)
                    {
                        bill.Status = BillStatus.Overdue;
                        bill.UpdatedAt = DateTime.UtcNow;
                        billsUpdated++;
                    }
                    
                    await billsRepository.UpdateAsync(bill);
                }
                
                if (billsUpdated > 0)
                {
                    _logger.LogInformation("Updated {Count} bills: {DueCount} due and {OverdueCount} overdue",
                        billsUpdated,
                        upcomingBills.Count(b => b.Status == BillStatus.Due),
                        upcomingBills.Count(b => b.Status == BillStatus.Overdue));
                }
                else
                {
                    _logger.LogInformation("No bills required status updates today");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating upcoming bills");
                throw;
            }
        }
    }
    
    private async Task UpdateDueBills()
    {
        using (IServiceScope scope = _serviceScopeFactory.CreateScope())
        {
            IBillsRepository billsRepository = scope.ServiceProvider.GetRequiredService<IBillsRepository>();

            try
            {
                var upcomingBills = await billsRepository.GetAllByStatusAsync(BillStatus.Due);
                var today = DateTime.UtcNow.Date;
                var billsUpdated = 0;

                foreach (var bill in upcomingBills)
                {
                    if (bill.DueDate.Date < today)
                    {
                        bill.Status = BillStatus.Overdue;
                        bill.UpdatedAt = today;
                        billsUpdated++;
                        
                        await billsRepository.UpdateAsync(bill);
                    }
                }

                if (billsUpdated > 0)
                {
                    await billsRepository.SaveChangesAsync();
                    _logger.LogInformation("Updated {Count} bills: {OverdueCount} overdue",
                        billsUpdated,
                        upcomingBills.Count(b => b.Status == BillStatus.Overdue));
                }
                else
                {
                    _logger.LogInformation("No overdue bills required status updates today");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating upcoming bills");
                throw;
            }
        }
    }
}