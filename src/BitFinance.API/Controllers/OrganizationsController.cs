using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitFinance.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/organizations")]
[ApiVersion("1.0")]
[Authorize]
public class OrganizationsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetOrganizations()
    {
        return Ok();
    }

    [HttpGet("{organizationId:guid}")]
    public IActionResult GetOrganizationById(Guid organizationId)
    {
        return Ok();
    }

    [HttpPost]
    public IActionResult CreateOrganization()
    {
        return Ok();
    }
    
    [HttpPatch("{organizationId:guid}")]
    public IActionResult UpdateOrganization(Guid organizationId)
    {
        return Ok();
    }

    [HttpPost("{organizationId:guid}/join")]
    public IActionResult JoinOrganization(Guid organizationId)
    {
        return Ok();
    }
}