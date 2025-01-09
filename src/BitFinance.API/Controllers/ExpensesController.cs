using System.Globalization;
using System.Security.Claims;
using Asp.Versioning;
using BitFinance.API.Models.Request;
using BitFinance.API.Models.Response;
using BitFinance.Business.Entities;
using BitFinance.Business.Enums;
using BitFinance.Data.Contexts;
using BitFinance.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BitFinance.API.Controllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/organizations/{organizationId}/expenses")]
public class ExpensesController : ControllerBase
{
    private readonly ILogger<ExpensesController> _logger;
    private readonly IExpensesRepository _expensesRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IOrganizationsRepository _organizationsRepository;

    public ExpensesController(
        ApplicationDbContext context, 
        ILogger<ExpensesController> logger, 
        IExpensesRepository expensesRepository, 
        IUsersRepository usersRepository, 
        IOrganizationsRepository organizationsRepository)
    {
        _logger = logger;
        _expensesRepository = expensesRepository;
        _usersRepository = usersRepository;
        _organizationsRepository = organizationsRepository;
    }
    
    [HttpGet]
    [Route("{expenseId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetExpenseResponse>> GetExpenseById([FromRoute] Guid organizationId, Guid expenseId)
    {
        try
        {
            var expense = await _expensesRepository.GetByIdAsync(organizationId, expenseId);

            if (expense is null)
            {
                return NotFound();
            }

            var response = new GetExpenseResponse();

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
    public async Task<ActionResult<CreateExpenseResponse>> CreateExpenseAsync([FromBody] CreateExpenseRequest request)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId)) return BadRequest("Invalid user");
            
            var user = await _usersRepository.GetByIdAsync(userId);
            
            if (user == null) return BadRequest("Invalid user");
            
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }
            
            var isValidCategory = Enum.TryParse(request.Category, true, out ExpenseCategory category);
            
            if (!isValidCategory) return UnprocessableEntity();
            
            var organization = await _organizationsRepository.GetByIdAsync(request.organizationId);
            
            if (organization is null) return BadRequest("Invalid organization");
            
            Expense expense = new()
            {
                Description = request.Description,
                Category = category,
                Amount = request.Amount,
                CreatedAt = DateTime.UtcNow,
                OrganizationId = organization.Id,
            };

            await _expensesRepository.CreateAsync(expense);
            
            var response = new CreateExpenseResponse
            {
                Id = expense.Id,
                Description = expense.Description,
                Category = expense.Category,
                Amount = expense.Amount,
                CreatedDate = expense.CreatedAt,
            };
            
            return CreatedAtAction(nameof(GetExpenseById), new { id = expense.Id }, response);
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
}