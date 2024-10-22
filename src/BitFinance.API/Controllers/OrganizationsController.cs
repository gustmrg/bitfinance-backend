using Asp.Versioning;
using BitFinance.API.Models.Request;
using BitFinance.Business.Entities;
using BitFinance.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitFinance.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/organizations")]
[ApiVersion("1.0")]
[Authorize]
public class OrganizationsController : ControllerBase
{
    private readonly IRepository<Organization, Guid> _repository;

    public OrganizationsController(IRepository<Organization, Guid> repository)
    {
        _repository = repository;
    }

    [HttpGet("{organizationId:guid}")]
    public async Task<IActionResult> GetOrganizationById(Guid organizationId)
    {
        var organization = await _repository.GetByIdAsync(organizationId);
        
        if (organization is null) return NotFound();
        
        return Ok(organization);
    }
    
    [HttpGet("users/{userId:guid}")]
    public IActionResult GetOrganizationsByUser(Guid userId)
    {
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrganization(CreateOrganizationRequest request)
    {
        var organization = new Organization
        {
            Name = request.Name,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
            DeletedAt = null,
        };
        
        await _repository.CreateAsync(organization);
        
        return CreatedAtAction(nameof(GetOrganizationById), new { id = organization.Id }, new { id = organization.Id, name = organization.Name, createdAt = organization.CreatedAt });
    }
    
    [HttpPatch("{organizationId:guid}")]
    public async Task<IActionResult> UpdateOrganization(Guid organizationId, [FromBody] UpdateOrganizationRequest request)
    {
        var organization = await _repository.GetByIdAsync(organizationId);
        
        if (organization is null) return NotFound();
        
        return Ok(organization);
    }

    [HttpPost("{organizationId:guid}/join")]
    public IActionResult JoinOrganization(Guid organizationId)
    {
        return Ok();
    }
}