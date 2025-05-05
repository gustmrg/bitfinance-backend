using BitFinance.API.Models.Bills;
using BitFinance.API.Models.Dashboard;
using BitFinance.API.Models.Expenses;
using BitFinance.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BitFinance.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/organizations/{organizationId:guid}/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IBillsService _billsService;
    private readonly IExpensesService _expensesService;

    public DashboardController(IBillsService billsService, IExpensesService expensesService)
    {
        _billsService = billsService;
        _expensesService = expensesService;
    }

    [HttpGet("upcoming-bills")]
    public async Task<ActionResult> GetUpcomingBills([FromRoute] Guid organizationId)
    {
        var bills = await _billsService.GetUpcomingBills(organizationId);

        var models = bills.Select(x => new BillResponse
        {
            Id = x.Id,
            Description = x.Description,
            Category = x.Category,
            Status = x.Status,
            DueDate = x.DueDate,
            AmountDue = x.AmountDue,
        }).ToList();

        var response = new UpcomingBillsResponse(models);
        
        return Ok(response);
    }
    
    [HttpGet("recent-expenses")]
    public async Task<ActionResult> GetRecentExpenses([FromRoute] Guid organizationId)
    {
        var expenses = await _expensesService.GetRecentExpenses(organizationId);
        
        var models = expenses.Select(x => new ExpenseResponse
        {
            Id = x.Id,
            Description = x.Description,
            Category = x.Category,
            Amount = x.Amount,
            OccurredAt = x.OccurredAt,
        }).ToList();
        
        var response = new RecentExpensesResponse(models);
        
        return Ok(response);
    }
}