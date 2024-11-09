using System.Security.Claims;
using Asp.Versioning;
using BitFinance.API.Models;
using BitFinance.API.Models.Request;
using BitFinance.API.Models.Response;
using BitFinance.Business.Entities;
using BitFinance.Data.Repositories;
using BitFinance.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.API.Controllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/organizations")]
public class OrganizationsController : ControllerBase
{
    private readonly IOrganizationsRepository _organizationsRepository;
    private readonly IUsersRepository _usersRepository;

    public OrganizationsController(IOrganizationsRepository organizationsRepository, IUsersRepository usersRepository)
    {
        _organizationsRepository = organizationsRepository;
        _usersRepository = usersRepository;
    }
    
    [HttpGet("organizations")]
    public async Task<IActionResult> GetOrganizations()
    {
        var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return BadRequest("Invalid user");
            
        var user = await _usersRepository.GetByIdAsync(userId);
            
        if (user == null) return BadRequest("Invalid user");
        
        var organizations = await _organizationsRepository.GetAllByUserIdAsync(userId);

        List<OrganizationResponseModel> response = [];

        foreach (var organization in organizations)
        {
            response.Add(new OrganizationResponseModel(organization.Id, organization.Name));
        }
        
        return Ok(response);
    }

    [HttpGet("{organizationId:guid}")]
    public async Task<IActionResult> GetOrganizationById(Guid organizationId)
    {
        var organization = await _organizationsRepository.GetByIdAsync(organizationId);
        
        if (organization is null) return NotFound();

        var response = new GetOrganizationByIdResponse
        {
            Id = organization.Id,
            Name = organization.Name,
            CreatedAt = organization.CreatedAt,
            UpdatedAt = organization.UpdatedAt,
        };

        foreach (var member in organization.Members)
        {
            response.Members.Add(new UserResponseModel(member.Id, member.UserName ?? string.Empty, member.Email ?? string.Empty));
        }
        
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrganization(CreateOrganizationRequest request)
    {
        var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return BadRequest("Invalid user");
            
        var user = await _usersRepository.GetByIdAsync(userId);
            
        if (user == null) return BadRequest("Invalid user");
        
        var organization = new Organization
        {
            Name = request.Name,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
            DeletedAt = null,
        };
        
        organization.Members.Add(user);
        
        await _organizationsRepository.CreateAsync(organization);
        
        return CreatedAtAction(nameof(GetOrganizationById), new { id = organization.Id }, new { id = organization.Id, name = organization.Name, createdAt = organization.CreatedAt });
    }
    
    [HttpPatch("{organizationId:guid}")]
    public async Task<IActionResult> UpdateOrganization(Guid organizationId, [FromBody] UpdateOrganizationRequest request)
    {
        var organization = await _organizationsRepository.GetByIdAsync(organizationId);
        
        if (organization is null) return NotFound();
        
        return Ok(organization);
    }

    [HttpPost("{organizationId:guid}/join")]
    public async Task<IActionResult> JoinOrganization(Guid organizationId)
    {
        var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return BadRequest("Invalid user");
            
        var user = await _usersRepository.GetByIdAsync(userId);
            
        if (user == null) return BadRequest("Invalid user");
        
        var organization = await _organizationsRepository.GetByIdAsync(organizationId);
        
        if (organization is null) return NotFound();
        
        organization.Members.Add(user);
        await _organizationsRepository.UpdateAsync(organization);
        
        return Ok();
    }
}