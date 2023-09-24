using BitFinance.API.Data;
using BitFinance.API.Models;
using BitFinance.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BillsController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public BillsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> Get()
    {
        var bills = await _dbContext.Bills.AsNoTracking().ToListAsync();
        return Results.Ok(bills);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IResult> Create(BillDTO billDto)
    {
        if (!ModelState.IsValid)
            return Results.UnprocessableEntity();

        try
        {
            var bill = new Bill()
            {
                Id = Guid.NewGuid(),
                Name = billDto.Name,
                Category = billDto.Category,
                AmountDue = billDto.AmountDue,
                AmountPaid = billDto.AmountPaid,
                CreatedDate = DateTime.Now,
                DueDate = billDto.DueDate,
                PaidDate = billDto.PaidDate
            };

            if (billDto.AmountPaid > 0 && billDto.PaidDate != null)
                bill.IsPaid = true;

            _dbContext.Bills.Add(bill);
            await _dbContext.SaveChangesAsync();
            return Results.Created($"/{bill.Id}", bill);
        }
        catch (Exception)
        {
            return Results.BadRequest();
        }
    }
}