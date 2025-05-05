using System.Globalization;
using Asp.Versioning;
using BitFinance.API.Attributes;
using BitFinance.API.Models;
using BitFinance.API.Models.Common;
using BitFinance.API.Models.Expenses;
using BitFinance.Domain.Entities;
using BitFinance.Domain.Enums;
using BitFinance.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BitFinance.API.Controllers;

[ApiController]
[Authorize]
[OrganizationAuthorization]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/organizations/{organizationId:guid}/expenses")]
public class ExpensesController : ControllerBase
{
    private readonly ILogger<ExpensesController> _logger;
    private readonly IExpensesRepository _expensesRepository;

    public ExpensesController(
        ILogger<ExpensesController> logger, 
        IExpensesRepository expensesRepository)
    {
        _logger = logger;
        _expensesRepository = expensesRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PagedResponse<Expense>>> GetExpenses(
        [FromRoute] Guid organizationId, 
        [FromQuery] GetExpensesRequest request)
    {
        var totalRecords = await _expensesRepository.GetEntriesCountAsync();
        var totalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);
        var expenses = await _expensesRepository.GetAllByOrganizationAsync(organizationId, 
            request.Page, request.PageSize, request.From, request.To);
        var expensesDto = expenses.Select(expense => new Models.Expenses.ExpenseResponse
        {
            Id = expense.Id,
            Amount = expense.Amount,
            Category = expense.Category,
            Description = expense.Description,
            Status = expense.Status,
            OccurredAt = expense.OccurredAt,
            CreatedBy = expense.CreatedByUser.FullName,
        }).ToList();

        return Ok(new PagedResponse<ExpenseResponse>(expensesDto, request.Page, request.PageSize, totalRecords, totalPages));
    }
    
    [HttpGet]
    [Route("{expenseId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExpenseResponse>> GetExpenseById([FromRoute] Guid organizationId, [FromRoute] Guid expenseId)
    {
        try
        {
            var expense = await _expensesRepository.GetByIdAsync(expenseId);

            if (expense is null)
            {
                return NotFound();
            }

            var response = new ExpenseResponse
            {
                Id = expense.Id,
                Amount = expense.Amount,
                Category = expense.Category,
                Description = expense.Description,
                Status = expense.Status,
                OccurredAt = expense.OccurredAt,
                CreatedBy = expense.CreatedByUser.FullName,
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            Log.Error("{Timestamp} - Error on {MethodName} method request: {Message}",
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture), 
                nameof(GetExpenseById), 
                ex.Message);
            return BadRequest();
        }
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<CreateExpenseResponse>> CreateExpenseAsync(
        [FromRoute] Guid organizationId,
        [FromBody] CreateExpenseRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }
            
            var isValidCategory = Enum.TryParse(request.Category, true, out ExpenseCategory category);
            if (!isValidCategory) return UnprocessableEntity();
            
            var isValidStatus = Enum.TryParse(request.Status, true, out ExpenseStatus status);
            if (!isValidStatus) return UnprocessableEntity();
            
            Expense expense = new()
            {
                Description = request.Description,
                Category = category,
                Amount = request.Amount,
                Status = status,
                OccurredAt = request.OccurredAt ?? DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = request.CreatedBy,
                OrganizationId = organizationId,
            };

            await _expensesRepository.CreateAsync(expense);
            
            var response = new CreateExpenseResponse
            {
                Id = expense.Id,
                Description = expense.Description,
                Category = expense.Category,
                Amount = expense.Amount,
                Status = expense.Status,
                OccurredAt = expense.OccurredAt,
                CreatedBy = expense.CreatedByUser.FullName,
            };
            
            return CreatedAtAction(nameof(GetExpenseById), new { expenseId = expense.Id, organizationId = expense.OrganizationId }, response);
        }
        catch (Exception ex)
        {
            Log.Error("{Timestamp} - Error on {MethodName} method request: {Message}",
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture), 
                nameof(CreateExpenseAsync), 
                ex.Message);
            return BadRequest();
        }
    }

    [HttpPatch]
    [Route("{expenseId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<UpdateExpenseResponse>> UpdateExpense([FromRoute] Guid expenseId, [FromBody] UpdateExpenseRequest request)
    {
        try
        {
            var isValidCategory = Enum.TryParse(request.Category, true, out ExpenseCategory category);
            if (!isValidCategory) return UnprocessableEntity();
            
            var isValidStatus = Enum.TryParse(request.Status, true, out ExpenseStatus status);
            if (!isValidStatus) return UnprocessableEntity();
            
            var expense = await _expensesRepository.GetByIdAsync(expenseId);

            if (expense is null) return NotFound();
            
            expense.Description = request.Description;
            expense.Category = category;
            expense.Amount = request.Amount;
            expense.Status = status;
            expense.OccurredAt = request.OccurredAt ?? DateTime.UtcNow;
            expense.UpdatedAt = DateTime.UtcNow;
            
            await _expensesRepository.UpdateAsync(expense);

            return Ok(new UpdateExpenseResponse(expense.Id, expense.Description, expense.Category, expense.Amount, expense.Status, expense.OccurredAt, expense.CreatedByUser.FullName));
        }
        catch (Exception ex)
        {
            Log.Error("{Timestamp} - Error on {MethodName} method request: {Message}",
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture), 
                nameof(UpdateExpense), 
                ex.Message);
            return BadRequest();
        }
    }

    [HttpDelete]
    [Route("{expenseId:guid}")]
    public async Task<ActionResult> DeleteExpenseById(Guid expenseId)
    {
        try
        {
            Expense? expense = await _expensesRepository.GetByIdAsync(expenseId);

            if (expense is null)
            {
                return NotFound();
            }

            if (expense.DeletedAt is not null)
            {
                return BadRequest();
            }

            await _expensesRepository.DeleteAsync(expense);

            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Error("{Timestamp} - Error on {MethodName} method request: {Message}",
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture), 
                nameof(DeleteExpenseById), 
                ex.Message);
            return BadRequest();
        }
    }
}