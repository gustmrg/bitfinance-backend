using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitFinance.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/bills")]
[ApiVersion("1.0")]
[Authorize]
public class OrganizationsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetOrganizations()
    {
        return Ok();
    }

    [HttpGet]
    public IActionResult GetOrganizationById(Guid organizationId)
    {
        return Ok();
    }

    [HttpPost]
    public IActionResult CreateOrganization()
    {
        return Ok();
    }
    
    [HttpPatch]
    public IActionResult UpdateOrganization()
    {
        return Ok();
    }
}