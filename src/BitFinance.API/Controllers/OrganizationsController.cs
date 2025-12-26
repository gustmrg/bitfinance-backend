using System.Security.Claims;
using Asp.Versioning;
using BitFinance.API.Attributes;
using BitFinance.Application.DTOs;
using BitFinance.Domain.Entities;
using BitFinance.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitFinance.API.Controllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/organizations")]
public class OrganizationsController : ControllerBase
{
    private readonly IOrganizationsRepository _organizationsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IOrganizationInvitesRepository _invitesRepository;

    public OrganizationsController(
        IOrganizationsRepository organizationsRepository,
        IUsersRepository usersRepository, 
        IOrganizationInvitesRepository invitesRepository)
    {
        _organizationsRepository = organizationsRepository;
        _usersRepository = usersRepository;
        _invitesRepository = invitesRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetOrganizations()
    {
        var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return BadRequest("Invalid user");
            
        var user = await _usersRepository.GetByIdAsync(userId);
            
        if (user == null) return BadRequest("Invalid user");
        
        var organizations = await _organizationsRepository.GetAllByUserIdAsync(userId);

        List<OrganizationSummary> response = [];

        foreach (var organization in organizations)
        {
            response.Add(new OrganizationSummary(organization.Id, organization.Name));
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
            response.Members.Add(new UserSummary(member.Id, member.FullName, member.Email ?? string.Empty, member.UserName ?? string.Empty));
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
        
        return CreatedAtAction(nameof(GetOrganizationById), new { organizationId = organization.Id }, new OrganizationSummary(organization.Id, organization.Name));
    }
    
    [HttpPatch("{organizationId:guid}")]
    public async Task<IActionResult> UpdateOrganization(Guid organizationId, [FromBody] UpdateOrganizationRequest request)
    {
        var organization = await _organizationsRepository.GetByIdAsync(organizationId);
        
        if (organization is null) return NotFound();
        
        return Ok(organization);
    }
    
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
        
        var organization = await _organizationsRepository.GetByIdAsync(invite.OrganizationId);
        
        if (organization is null) return NotFound("Organization not found");
        
        if (organization.Members.Contains(user)) return BadRequest("You cannot join this organization");
        
        if (invite.ExpiresAt < DateTime.UtcNow) return BadRequest("This invite has expired");
        
        organization.Members.Add(user);
        await _organizationsRepository.UpdateAsync(organization);
        return Ok();
    }
}