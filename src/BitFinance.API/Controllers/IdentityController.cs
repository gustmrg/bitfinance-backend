using System.Security.Claims;
using Asp.Versioning;
using BitFinance.API.Models;
using BitFinance.API.Models.Response;
using BitFinance.Business.Entities;
using BitFinance.Data.Repositories.Interfaces;
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
    private readonly IUsersRepository _usersRepository;

    public IdentityController(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetMe()
    {
        var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return BadRequest("Invalid user");
            
        var user = await _usersRepository.GetByIdAsync(userId);
            
        if (user == null) return BadRequest("Invalid user");

        List<OrganizationResponseModel> organizations = [];

        foreach (var organization in user.Organizations)
        {
            organizations.Add(new OrganizationResponseModel(organization.Id, organization.Name));
        }
        
        var response = new GetMeResponse(user.Id, user.UserName ?? string.Empty, user.Email ?? string.Empty, organizations);
        
        return Ok(response);
    }
}