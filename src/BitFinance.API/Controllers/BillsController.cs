using BitFinance.API.Models;
using BitFinance.Domain.Entities;
using BitFinance.Infrastructure.Data;
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
    public void Get() { }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IResult> CreateBillAsync([FromBody] BillInputModel model)
    {
        if (!ModelState.IsValid)
            return Results.UnprocessableEntity();

        var bill = new Bill
        {
            Name = model.Name,
            Category = model.Category,
            CreatedDate = DateTime.UtcNow,
            DueDate = model.DueDate.ToUniversalTime(),
            PaidDate = model.PaidDate?.ToUniversalTime(),
            AmountDue = model.AmountDue,
            AmountPaid = model.AmountPaid,
            IsPaid = false,
            IsDeleted = false
        };

        _context.Bills.Add(bill);
        await _context.SaveChangesAsync();

        string uri = $"/bills/" + bill.Id;
        
        return Results.Created(uri, bill);
    }
}