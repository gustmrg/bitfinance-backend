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
    public IActionResult CreateOrganization(CreateOrganizationRequest request)
    {
        return Ok();
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