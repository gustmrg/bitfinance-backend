using BitFinance.API.Data;
using BitFinance.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.API.Controllers;

[ApiController]
[Route("[controller]")]
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

        return Results.Ok(bills);
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IResult> GetBillById(int id)
    { 
        var bill = await _context.Bills.FirstOrDefaultAsync(x => x.Id == id);

        return Results.Ok(bill);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IResult> CreateBillAsync([FromBody] BillDTO model)
    {
        if (!ModelState.IsValid)
            return Results.UnprocessableEntity();

        var bill = new Bill
        {
            Name = model.Name,
            Category = model.Category,
            CreatedDate = DateTime.UtcNow.AddHours(-3),
            DueDate = model.DueDate.ToUniversalTime(),
            PaidDate = model.PaidDate?.ToUniversalTime(),
            AmountDue = model.AmountDue,
            AmountPaid = model.AmountPaid,
            IsPaid = model.IsPaid,
            IsDeleted = false
        };

        _context.Bills.Add(bill);
        await _context.SaveChangesAsync();

        string uri = $"/bills/" + bill.Id;
        
        return Results.Created(uri, bill);
    }
    
    [HttpPost]
    [Route("{id:int}")]
    public async Task<IResult> UpdateBillById(int id, BillDTO billDTO)
    {
        var bill = await _context.Bills.FirstOrDefaultAsync(x => x.Id == id);

        if (bill is null)
            return Results.NotFound("Could not find the requested Bill");

        bill.Name = billDTO.Name;
        bill.Category = billDTO.Category;
        bill.DueDate = billDTO.DueDate.ToUniversalTime();
        bill.PaidDate = billDTO.PaidDate?.ToUniversalTime();
        bill.AmountDue = billDTO.AmountDue;
        bill.AmountPaid = billDTO.AmountPaid;
        bill.IsPaid = billDTO.IsPaid;
        bill.IsDeleted = billDTO.IsDeleted;

        _context.Bills.Update(bill);
        await _context.SaveChangesAsync();
        
        return Results.Ok(bill);
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IResult> DeleteBillById(int id)
    { 
        var bill = await _context.Bills.FirstOrDefaultAsync(x => x.Id == id);
        
        if (bill is null)
            return Results.NotFound("Could not find the requested Bill");

        bill.IsDeleted = true;
        _context.Bills.Update(bill);
        await _context.SaveChangesAsync();

        return Results.Ok("Bill deleted");
    }
}