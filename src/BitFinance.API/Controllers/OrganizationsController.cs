using System.Security.Claims;
using Asp.Versioning;
using BitFinance.API.Attributes;
using BitFinance.API.Models;
using BitFinance.API.Models.Request;
using BitFinance.API.Models.Response;
using BitFinance.Business.Entities;
using BitFinance.Business.Enums;
using BitFinance.Data.Repositories.Interfaces;
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
    private readonly IInvitationsRepository _invitationsRepository;

    public OrganizationsController(
        IOrganizationsRepository organizationsRepository,
        IUsersRepository usersRepository,
        IInvitationsRepository invitationsRepository)
    {
        _organizationsRepository = organizationsRepository;
        _usersRepository = usersRepository;
        _invitationsRepository = invitationsRepository;
    }

    [HttpGet]
    [EndpointSummary("List user organizations")]
    [EndpointDescription("Returns all organizations the authenticated user is a member of.")]
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
    [EndpointSummary("Get organization details")]
    [EndpointDescription("Returns the details and member list of a specific organization.")]
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

        foreach (var membership in organization.Members)
        {
            var user = membership.User;
            response.Members.Add(new UserResponseModel(user.Id, user.UserName ?? string.Empty, user.Email ?? string.Empty));
        }

        return Ok(response);
    }

    [HttpPost]
    [EndpointSummary("Create an organization")]
    [EndpointDescription("Creates a new organization and adds the authenticated user as the first member.")]
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
        };

        organization.Members.Add(new OrganizationMember
        {
            UserId = user.Id,
            OrganizationId = organization.Id,
            Role = OrgRole.Owner,
            JoinedAt = DateTime.UtcNow,
        });

        await _organizationsRepository.CreateAsync(organization);

        return CreatedAtAction(nameof(GetOrganizationById), new { organizationId = organization.Id }, new OrganizationResponseModel(organization.Id, organization.Name));
    }

    [HttpPatch("{organizationId:guid}")]
    [EndpointSummary("Update an organization")]
    [EndpointDescription("Updates the details of a specific organization.")]
    public async Task<IActionResult> UpdateOrganization(Guid organizationId, [FromBody] UpdateOrganizationRequest request)
    {
        var organization = await _organizationsRepository.GetByIdAsync(organizationId);

        if (organization is null) return NotFound();

        return Ok(organization);
    }

    [HttpPost("{organizationId:guid}/invite")]
    [OrganizationAuthorization]
    [EndpointSummary("Create an invitation")]
    [EndpointDescription("Creates an invitation for a user to join the organization, valid for 24 hours.")]
    public async Task<IActionResult> CreateInvite([FromRoute] Guid organizationId, [FromBody] CreateInvitationRequest request)
    {
        var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return BadRequest("Invalid user");

        var invitation = new Invitation
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            Email = request.Email,
            Role = request.Role ?? OrgRole.Member,
            InvitedByUserId = userId,
            Status = InvitationStatus.Pending,
            Token = Guid.NewGuid().ToString("N"),
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            CreatedAt = DateTime.UtcNow,
        };

        await _invitationsRepository.CreateAsync(invitation);

        return Ok(new CreateInvitationResponse(invitation.Id, invitation.Token, invitation.ExpiresAt));
    }

    [HttpPost("join")]
    [EndpointSummary("Join an organization")]
    [EndpointDescription("Adds the authenticated user to an organization using a valid invitation token.")]
    public async Task<IActionResult> JoinOrganization([FromBody] JoinOrganizationRequest request)
    {
        var userId = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;
        var user = await _usersRepository.GetByIdAsync(userId!);

        if (user == null) return NotFound("Invalid user");

        var invitation = await _invitationsRepository.GetByTokenAsync(request.Token);

        if (invitation is null) return NotFound("Invalid invitation");

        if (invitation.Status != InvitationStatus.Pending) return BadRequest("This invitation is no longer valid");

        if (invitation.ExpiresAt < DateTime.UtcNow) return BadRequest("This invitation has expired");

        var organization = invitation.Organization;

        if (organization is null) return NotFound("Organization not found");

        if (organization.Members.Any(m => m.UserId == user.Id))
            return BadRequest("You are already a member of this organization");

        organization.Members.Add(new OrganizationMember
        {
            UserId = user.Id,
            OrganizationId = organization.Id,
            Role = invitation.Role,
            JoinedAt = DateTime.UtcNow,
        });

        invitation.Status = InvitationStatus.Accepted;

        await _organizationsRepository.UpdateAsync(organization);
        await _invitationsRepository.UpdateAsync(invitation);

        return Ok();
    }
}
