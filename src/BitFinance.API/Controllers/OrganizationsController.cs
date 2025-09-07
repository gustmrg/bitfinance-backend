using System.Security.Claims;
using BitFinance.API.Models;
using BitFinance.API.Models.Request;
using BitFinance.API.Models.Response;
using BitFinance.Application.Interfaces;
using BitFinance.Domain.Entities;
using BitFinance.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitFinance.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class OrganizationsController : ControllerBase
{
    /*
    private readonly IUserRepository _userRepository;
    private readonly IOrganizationService _organizationService;

    public OrganizationsController(
        IUserRepository userRepository, 
        IOrganizationService organizationService)
    {
        _usersRepository = usersRepository;
        _organizationService = organizationService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetOrganizations()
    {
        var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return BadRequest("Invalid user");
            
        var user = await _usersRepository.GetByIdAsync(userId);
            
        if (user == null) return BadRequest("Invalid user");

        var organizations = await _organizationService.GetByUserIdAsync(userId);

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
        var organization = await _organizationService.GetByIdAsync(organizationId);
        
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

        var organization = new Organization(request.Name);
        
        organization.Members.Add(user);
        
        await _organizationService.CreateAsync(organization);
        
        return CreatedAtAction(nameof(GetOrganizationById), new { organizationId = organization.Id }, new OrganizationResponseModel(organization.Id, organization.Name));
    }
    
    [HttpPatch("{organizationId:guid}")]
    public async Task<IActionResult> UpdateOrganization(Guid organizationId, [FromBody] UpdateOrganizationRequest request)
    {
        var organization = await _organizationService.GetByIdAsync(organizationId);
        
        if (organization is null) return NotFound();
        
        return Ok(organization);
    }
    
    // Disabled for invites refactoring
    /*
    [HttpPost("{organizationId:guid}/invite")]
    [OrganizationAuthorization]
    public async Task<IActionResult> CreateInvite([FromRoute] Guid organizationId)
    {
        var invite = new OrganizationInvite
        {
            OrganizationId = organizationId,
            ExpiresAt = DateTime.UtcNow.AddDays(1),
        };
        
        await _invitesRepository.CreateAsync(invite);
    
        return Ok(new CreateOrganizationInviteResponse(invite.Id, invite.ExpiresAt));
    }

    [HttpPost("join")]
    public async Task<IActionResult> JoinOrganization([FromBody] JoinOrganizationRequest request)
    {
        var userId = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;
        var user = await _usersRepository.GetByIdAsync(userId!);
        
        if (user == null) return NotFound("Invalid user");
        
        var invite = await _invitesRepository.GetByIdAsync(request.InviteId);
        
        if (invite is null) return NotFound("Invalid invite");
        
        var organization = await _organizationService.GetByIdAsync(invite.OrganizationId);
        
        if (organization is null) return NotFound("Organization not found");
        
        if (organization.Members.Contains(user)) return BadRequest("You cannot join this organization");
        
        if (invite.ExpiresAt < DateTime.UtcNow) return BadRequest("This invite has expired");
        
        organization.Members.Add(user);
        await _organizationService.UpdateAsync(organization);
        return Ok();
    }
    */
}