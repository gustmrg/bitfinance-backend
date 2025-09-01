using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitFinance.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContextController : ControllerBase
{
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentContext()
    {
        return Ok();
    }

    [HttpPost("organization")]
    public async Task<IActionResult> SetOrganizationContext()
    {
        return Ok();
    }

    [HttpDelete("session")]
    public async Task<IActionResult> DeleteSession()
    {
        return Ok();
    }
}