using System.Globalization;
using System.Text.Json;
using BitFinance.API.Models;
using BitFinance.API.Services;
using BitFinance.Business.Entities;
using BitFinance.Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.API.Controllers;

[ApiController]
[Route("bills")]
[Authorize]
public class BillsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BillsController> _logger;
    private readonly ICacheService _cache;
    
    public BillsController(ApplicationDbContext context, ILogger<BillsController> logger, ICacheService cache)
    {
        _context = context;
        _logger = logger;
        _cache = cache;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<GetBillResponse>>> GetBillsAsync()
    {
        try
        {
            IEnumerable<Bill>? bills;
            var key = _cache.GenerateKey<IEnumerable<Bill>>(string.Empty);

            var billsListCached = await _cache.GetAsync(key);

            if (!string.IsNullOrWhiteSpace(billsListCached))
            {
                bills = JsonSerializer.Deserialize<List<Bill>>(billsListCached);
            }
            else
            {
                bills = await _context.Bills.AsNoTracking().ToListAsync();

                if (bills.Any())
                {
                    await _cache.SetAsync(key, JsonSerializer.Serialize(bills));
                }
            }
            
            var response = bills?.Select(bill => new GetBillResponse
                {
                    Id = bill.Id,
                    Name = bill.Name,
                    Category = bill.Category,
                    CreatedDate = bill.CreatedDate,
                    DueDate = bill.DueDate,
                    PaidDate = bill.PaidDate,
                    AmountDue = bill.AmountDue,
                    AmountPaid = bill.AmountPaid
                })
                .ToList();
            
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogInformation("{Time} - Error on {MethodName} method request: {Message}", 
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture), nameof(GetBillsAsync), e.Message);
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
            Bill? bill;
            string key = _cache.GenerateKey<Bill>(id.ToString());
            
            var billCache = await _cache.GetAsync(key);

            if (!string.IsNullOrWhiteSpace(billCache))
            {
                bill = JsonSerializer.Deserialize<Bill>(billCache);
            }
            else
            {
                bill = await _context.Bills.FirstOrDefaultAsync(b => b.Id == id);    
            }
            
            if (bill is null)
            {
                return NotFound();
            }

            await _cache.SetAsync(key, JsonSerializer.Serialize(bill));
            
            var response = new GetBillResponse
            {
                Id = bill.Id,
                Name = bill.Name,
                Category = bill.Category,
                CreatedDate = bill.CreatedDate,
                DueDate = bill.DueDate,
                PaidDate = bill.PaidDate,
                AmountDue = bill.AmountDue,
                AmountPaid = bill.AmountPaid
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogInformation("{Time} - Error on {MethodName} method request: {Message}", 
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture), nameof(GetBillById), ex.Message);
            return BadRequest();
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<CreateBillResponse>> CreateBillAsync([FromBody] CreateBillRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity();

            var bill = new Bill
            {
                Name = request.Name,
                Category = request.Category,
                CreatedDate = DateTime.UtcNow.AddHours(-3),
                DueDate = request.DueDate.ToUniversalTime(),
                PaidDate = request.PaidDate?.ToUniversalTime(),
                AmountDue = request.AmountDue,
                AmountPaid = request.AmountPaid,
                IsPaid = request.AmountPaid is not null,
                IsDeleted = false
            };

            _context.Bills.Add(bill);
            await _context.SaveChangesAsync();
            
            string key = _cache.GenerateKey<Bill>(bill.Id.ToString());
            await _cache.SetAsync(key, JsonSerializer.Serialize(bill));

            var response = new CreateBillResponse { Id = bill.Id };
            
            return CreatedAtAction(nameof(GetBillById), new { id = bill.Id }, response);
        }
        catch (Exception e)
        {
            _logger.LogInformation("{Time} - Error on {MethodName} method request: {Message}", 
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture), nameof(CreateBillAsync), e.Message);
            return BadRequest();
        }
    }
    
    [HttpPut]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<UpdateBillResponse>> UpdateBillById(Guid id, [FromBody] UpdateBillRequest request)
    {
        try
        {
            Bill? bill;
            string key = _cache.GenerateKey<Bill>(id.ToString());
            
            var billCache = await _cache.GetAsync(key);
            
            if (!string.IsNullOrWhiteSpace(billCache))
            {
                await _cache.RemoveAsync(key);
            }
            
            bill = await _context.Bills.FirstOrDefaultAsync(b => b.Id == id);

            if (bill is null)
                return NotFound($"Could not find the requested Bill for id {id}");

            bill.Name = request.Name;
            bill.Category = request.Category;
            bill.DueDate = request.DueDate.ToUniversalTime();
            bill.PaidDate = request.PaidDate?.ToUniversalTime();
            bill.AmountDue = request.AmountDue;
            bill.AmountPaid = request.AmountPaid;
            bill.IsPaid = request.AmountPaid is not null;

            _context.Bills.Update(bill);
            await _context.SaveChangesAsync();
            await _cache.SetAsync(key, JsonSerializer.Serialize(bill));

            var response = new UpdateBillResponse
            {
                Id = bill.Id,
                Name = bill.Name,
                Category = bill.Category,
                DueDate = bill.DueDate,
                PaidDate = bill.PaidDate,
                AmountDue = bill.AmountDue,
                AmountPaid = bill.AmountPaid,
                IsPaid = bill.IsPaid
            };
        
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogInformation("{Time} - Error on {MethodName} method request: {Message}", 
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture), nameof(UpdateBillById), e.Message);
            return BadRequest();
        }
    }
    
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<ActionResult<DeleteBillResponse>> DeleteBillById(Guid id)
    {
        try
        {
            var bill = await _context.Bills.FirstOrDefaultAsync(x => x.Id == id && x.DeletedDate == null);

            if (bill is null)
            {
                return NotFound("Could not find the requested Bill");
            }
            
            bill.DeletedDate = DateTime.UtcNow.AddHours(-3);
            bill.IsDeleted = true;
            _context.Bills.Update(bill);
            await _context.SaveChangesAsync();
            
            var key = _cache.GenerateKey<Bill>(id.ToString());
            var billCache = await _cache.GetAsync(key);
            
            if (!string.IsNullOrWhiteSpace(billCache))
            {
                await _cache.RemoveAsync(key);
            }

            var response = new DeleteBillResponse
            {
                Id = bill.Id,
                IsDeleted = bill.IsDeleted
            };

            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogInformation("{Time} - Error on {MethodName} method request: {Message}", 
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture), nameof(DeleteBillById), e.Message);
            return BadRequest();
        }
    }
}