using System.Security.Claims;
using Asp.Versioning;
using BitFinance.API.InputModels;
using BitFinance.API.Services.Interfaces;
using BitFinance.API.ViewModels;
using BitFinance.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitFinance.API.Controllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/identity")]
public class IdentityController : ControllerBase
{
    private readonly IUsersService _usersService;

    public IdentityController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return BadRequest("Invalid user");
            
        var user = await _usersService.GetUserByIdAsync(userId);
            
        if (user == null) return BadRequest("Invalid user");

        List<OrganizationViewModel> organizations = [];
        organizations.AddRange(user.Organizations.Select(organization => new OrganizationViewModel(organization.Id, organization.Name)));
        
        return Ok(new UserViewModel(user.Id, user.FirstName, user.LastName, user?.Email ?? string.Empty, ReplaceUserName(user?.UserName), organizations));
    }

    [HttpPost("manage/profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileInputModel model)
    {
        var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return BadRequest("Invalid user");
            
        var user = await _usersService.GetUserByIdAsync(userId);
            
        if (user == null) return BadRequest("Invalid user");
        
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        
        await  _usersService.UpdateUserAsync(user);
        
        List<OrganizationViewModel> organizations = [];
        organizations.AddRange(user.Organizations.Select(organization => new OrganizationViewModel(organization.Id, organization.Name)));

        return Ok(new UserViewModel(user.Id, user.FirstName, user.LastName, user.Email ?? string.Empty, ReplaceUserName(user?.UserName), organizations));
    }

    private static string ReplaceUserName(string? email)
    {
        return string.IsNullOrEmpty(email) ? string.Empty : email.Split('@')[0];
    }
}