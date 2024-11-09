using System.Security.Claims;
using Asp.Versioning;
using BitFinance.API.Models.Response;
using BitFinance.Business.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.API.Controllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/identity/me")]
public class IdentityController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public IdentityController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetMe()
    {
        var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return BadRequest("Invalid user");
            
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            
        if (user == null) return BadRequest("Invalid user");
        
        var response = new GetMeResponse(user.Id, user.UserName ?? string.Empty, user.Email ?? string.Empty);
        
        return Ok(response);
    }
}