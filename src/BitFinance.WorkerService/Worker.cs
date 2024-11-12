using BitFinance.Business.Entities;
using BitFinance.Business.Enums;
using BitFinance.Data.Repositories.Interfaces;

namespace BitFinance.WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker started running at: {time}", DateTimeOffset.Now);
            }

            var currentTime = DateTime.Now;
            var midnight = DateTime.Today.AddDays(-1);
            var delay = midnight - currentTime;
            
            await Task.Delay(delay, stoppingToken);

            using (var scope = _serviceProvider.CreateScope())
            {
                var billsRepository = scope.ServiceProvider.GetService<IBillsRepository>();

                if (billsRepository is not null)
                {
                    try
                    {
                        var bills = await billsRepository.GetAllAsync();
                        var billsToProcess = bills.Where(x => x.DueDate.Date == DateTime.Today.Date 
                                                              && x.Status == BillStatus.Upcoming).ToList();
                        
                        if (billsToProcess.Count > 0)
                        {
                            foreach (var bill in billsToProcess)
                            {
                                bill.Status = BillStatus.Due;
                                await billsRepository.UpdateAsync(bill, b => b.Status);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Error while updating bill: {Message}", e.Message);
                    }
                }
            }
            
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker stopped running at: {time}", DateTimeOffset.Now);
            }
        }
    }
}