using System.Globalization;
using System.Security.Claims;
using Asp.Versioning;
using BitFinance.API.Models;
using BitFinance.API.Models.Request;
using BitFinance.API.Models.Response;
using BitFinance.Business.Entities;
using BitFinance.Business.Enums;
using BitFinance.Data.Contexts;
using BitFinance.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BitFinance.API.Controllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/bills")]
public class BillsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BillsController> _logger;
    private readonly IBillsRepository _billsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IOrganizationsRepository _organizationsRepository;
    
    public BillsController(ApplicationDbContext context, 
        ILogger<BillsController> logger, 
        IBillsRepository billsRepository, 
        IUsersRepository usersRepository, IOrganizationsRepository organizationsRepository)
    {
        _context = context;
        _logger = logger;
        _billsRepository = billsRepository;
        _usersRepository = usersRepository;
        _organizationsRepository = organizationsRepository;
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<CreateBillResponse>> CreateBillAsync([FromBody] CreateBillRequest request)
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
            
            var isValidCategory = Enum.TryParse(request.Category, true, out BillCategory category);
            var isValidStatus = Enum.TryParse(request.Status, true, out BillStatus status);
            
            if (!isValidCategory || !isValidStatus) return UnprocessableEntity();
            
            var organization = await _organizationsRepository.GetByIdAsync(request.organizationId);
            
            if (organization is null) return BadRequest("Invalid organization");
            
            Bill bill = new()
            {
                Description = request.Description,
                Category = category,
                Status = status,
                CreatedAt = DateTime.UtcNow,
                DueDate = request.DueDate.ToUniversalTime(),
                PaymentDate = request.PaymentDate?.ToUniversalTime(),
                AmountDue = request.AmountDue,
                AmountPaid = request.AmountPaid,
                OrganizationId = organization.Id,
            };

            await _billsRepository.CreateAsync(bill);
            
            var response = new CreateBillResponse
            {
                Id = bill.Id,
                Description = bill.Description,
                Category = bill.Category,
                Status = bill.Status,
                CreatedDate = bill.CreatedAt,
                DueDate = bill.DueDate,
                PaidDate = bill.PaymentDate,
                AmountDue = bill.AmountDue,
                AmountPaid = bill.AmountPaid
            };
            
            return CreatedAtAction(nameof(GetBillById), new { id = bill.Id }, response);
        }
        catch (Exception ex)
        {
            Log.Error("{Timestamp} - Error on {MethodName} method request: {Message}",
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture), 
                nameof(CreateBillAsync), 
                ex.Message);
            return BadRequest();
        }
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<GetBillResponse>> GetBillById([FromRoute] Guid id)
    {
        try
        {
            Bill? bill = await _billsRepository.GetByIdAsync(id);

            if (bill is null)
            {
                return NotFound();
            }

            var response = new GetBillResponse
            {
                Id = bill.Id,
                Description = bill.Description,
                Category = bill.Category,
                Status = bill.Status,
                CreatedAt = bill.CreatedAt,
                DueDate = bill.DueDate,
                PaymentDate = bill.PaymentDate,
                AmountDue = bill.AmountDue,
                AmountPaid = bill.AmountPaid
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            Log.Error("{Timestamp} - Error on {MethodName} method request: {Message}",
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture), 
                nameof(GetBillById), 
                ex.Message);
            return BadRequest();
        }
    }
    
    [HttpPatch]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<UpdateBillResponse>> UpdateBill(Guid id, [FromBody] UpdateBillRequest request)
    {
        try
        {
            bool isValidCategory = Enum.TryParse(request.Category, true, out BillCategory category);
            bool isValidStatus = Enum.TryParse(request.Status, true, out BillStatus status);
            
            if (!isValidCategory || !isValidStatus) return UnprocessableEntity();
            
            Bill? bill;
            
            bill = await _context.Bills.FirstOrDefaultAsync(b => b.Id == id && b.DeletedAt == null);

            if (bill is null)
            {
                return NotFound();
            }
            
            bill.Description = request.Description;
            bill.Category = category;
            bill.Status = status;
            bill.DueDate = request.DueDate.ToUniversalTime();
            bill.PaymentDate = request.PaymentDate?.ToUniversalTime();
            bill.AmountDue = request.AmountDue;
            bill.AmountPaid = request.AmountPaid;

            await _billsRepository.UpdateAsync(bill, 
                b => b.Description, 
                b => b.Category, 
                b => b.Status, 
                b => b.DueDate, 
                b => b.PaymentDate,
                b => b.AmountDue,
                b => b.AmountPaid);

            var response = new UpdateBillResponse
            {
                Id = bill.Id,
                Description = bill.Description,
                Category = bill.Category,
                Status = bill.Status,
                DueDate = bill.DueDate,
                PaidDate = bill.PaymentDate,
                AmountDue = bill.AmountDue,
                AmountPaid = bill.AmountPaid
            };
        
            return Ok(response);
        }
        catch (Exception ex)
        {
            Log.Error("{Timestamp} - Error on {MethodName} method request: {Message}",
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture), 
                nameof(UpdateBill), 
                ex.Message);
            return BadRequest();
        }
    }
    
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<ActionResult> DeleteBillById(Guid id)
    {
        try
        {
            Bill? bill = await _billsRepository.GetByIdAsync(id);

            if (bill is null)
            {
                return NotFound();
            }

            if (bill.DeletedAt is not null)
            {
                return BadRequest();
            }

            await _billsRepository.DeleteAsync(bill);

            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Error("{Timestamp} - Error on {MethodName} method request: {Message}",
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture), 
                nameof(DeleteBillById), 
                ex.Message);
            return BadRequest();
        }
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResponse<Bill>>> GetBillsAsync([FromBody] GetBillsRequest request)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId)) return BadRequest("Invalid user");

            var user = await _usersRepository.GetByIdAsync(userId);
            
            if (user == null) return BadRequest("Invalid user");
            
            if (user.Organizations.All(o => o.Id != request.OrganizationId)) return BadRequest("Invalid organization");

            var totalRecords = await _billsRepository.GetEntriesCountAsync();
            var bills = await _billsRepository.GetAllByOrganizationAsync(request.OrganizationId, request.Page, request.PageSize);

            var billsDto = bills.Select(bill => new GetBillResponse
                {
                    Id = bill.Id,
                    Description = bill.Description,
                    Category = bill.Category,
                    Status = bill.Status,
                    CreatedAt = bill.CreatedAt,
                    DueDate = bill.DueDate,
                    PaymentDate = bill.PaymentDate,
                    AmountDue = bill.AmountDue,
                    AmountPaid = bill.AmountPaid,
                    OrganizationId = bill.OrganizationId,
                })
                .ToList();
            
            var response = new PagedResponse<GetBillResponse>(billsDto, totalRecords, request.Page, request.PageSize);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("{Timestamp} - Error on {MethodName} method request: {Message}",
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture),
                nameof(GetBillsAsync),
                ex.Message);
            return BadRequest();
        }
    }
}