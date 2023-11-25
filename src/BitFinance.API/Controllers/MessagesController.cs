using BitFinance.API.Data;
using BitFinance.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BitFinance.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MessagesController : Controller
{
    private readonly ApplicationDbContext _context;

    public MessagesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IResult SendMessage(MessageDTO messageDto)
    {
        var message = new Message
        {
            From = messageDto.From,
            To = messageDto.To,
            Body = messageDto.Body,
            Platform = messageDto.Platform,
            CreatedDate = DateTime.UtcNow.AddHours(-3),
            ScheduledDate = messageDto.ScheduledDate ?? DateTime.UtcNow.AddHours(-3),
            IsSent = false
        };

        return Results.Ok(message);
    }
}