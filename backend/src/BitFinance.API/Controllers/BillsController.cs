using BitFinance.API.Models;
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
    
    public BillsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IResult> Get()
    { 
        var bills = await _context.Bills.AsNoTracking().ToListAsync();
        var response = bills.Select(bill => new GetBillResponse
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
        
        return Results.Ok(response);
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IResult> GetBillById(Guid id)
    { 
        var bill = await _context.Bills.FirstOrDefaultAsync(x => x.Id == id);
        
        if (bill is null)
            return Results.NotFound("Could not find the requested Bill");

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

        return Results.Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IResult> CreateBillAsync([FromBody] CreateBillRequest request)
    {
        if (!ModelState.IsValid)
            return Results.UnprocessableEntity();

        var bill = new Bill
        {
            Name = request.Name,
            Category = request.Category,
            CreatedDate = DateTime.UtcNow.AddHours(-3),
            DueDate = request.DueDate.ToUniversalTime(),
            PaidDate = request.PaidDate?.ToUniversalTime(),
            AmountDue = request.AmountDue,
            AmountPaid = request.AmountPaid,
            IsPaid = request.IsPaid,
            IsDeleted = false
        };

        _context.Bills.Add(bill);
        await _context.SaveChangesAsync();

        var uri = $"/bills/" + bill.Id;
        
        return Results.Created(uri, new CreateBillResponse { Id = bill.Id });
    }
    
    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IResult> UpdateBillById(Guid id, UpdateBillRequest request)
    {
        var bill = await _context.Bills.FirstOrDefaultAsync(x => x.Id == id);

        if (bill is null)
            return Results.NotFound("Could not find the requested Bill");

        bill.Name = request.Name;
        bill.Category = request.Category;
        bill.DueDate = request.DueDate.ToUniversalTime();
        bill.PaidDate = request.PaidDate?.ToUniversalTime();
        bill.AmountDue = request.AmountDue;
        bill.AmountPaid = request.AmountPaid;
        bill.IsPaid = request.IsPaid;
        bill.IsDeleted = request.IsDeleted ?? false;

        _context.Bills.Update(bill);
        await _context.SaveChangesAsync();

        var response = new UpdateBillResponse
        {
            Id = bill.Id,
            Name = bill.Name,
            Category = bill.Category,
            DueDate = bill.DueDate,
            PaidDate = bill.PaidDate,
            AmountDue = bill.AmountDue,
            AmountPaid = bill.AmountPaid,
            IsPaid = bill.IsPaid,
            IsDeleted = bill.IsDeleted
        };
        
        return Results.Ok(response);
    }
    
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IResult> DeleteBillById(Guid id)
    { 
        var bill = await _context.Bills.FirstOrDefaultAsync(x => x.Id == id);
        
        if (bill is null)
            return Results.NotFound("Could not find the requested Bill");

        bill.IsDeleted = true;
        _context.Bills.Update(bill);
        await _context.SaveChangesAsync();

        var response = new DeleteBillResponse
        {
            Id = bill.Id,
            IsDeleted = bill.IsDeleted
        };

        return Results.Ok(response);
    }
}