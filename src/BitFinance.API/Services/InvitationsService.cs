using BitFinance.API.Models;
using BitFinance.API.Services.Interfaces;
using BitFinance.Business.Entities;
using BitFinance.Business.Enums;
using BitFinance.Data.Repositories.Interfaces;

namespace BitFinance.API.Services;

public class InvitationsService : IInvitationsService
{
    private readonly IInvitationsRepository _invitationsRepository;
    private readonly IOrganizationsRepository _organizationsRepository;

    public InvitationsService(
        IInvitationsRepository invitationsRepository,
        IOrganizationsRepository organizationsRepository)
    {
        _invitationsRepository = invitationsRepository;
        _organizationsRepository = organizationsRepository;
    }

    public async Task<Invitation> CreateInvitationAsync(
        Guid organizationId, string email, OrgRole role, string invitedByUserId)
    {
        var invitation = new Invitation
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            Email = email,
            Role = role,
            InvitedByUserId = invitedByUserId,
            Status = InvitationStatus.Pending,
            Token = Guid.NewGuid().ToString("N"),
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            CreatedAt = DateTime.UtcNow,
        };

        await _invitationsRepository.CreateAsync(invitation);
        return invitation;
    }

    public async Task<JoinOrganizationResult> JoinOrganizationAsync(string token, string userId)
    {
        var invitation = await _invitationsRepository.GetByTokenAsync(token);

        if (invitation is null)
            return JoinOrganizationResult.Failed(JoinOrganizationError.InvalidToken, "Invalid invitation");

        if (invitation.Status != InvitationStatus.Pending)
            return JoinOrganizationResult.Failed(JoinOrganizationError.InvitationNotPending, "This invitation is no longer valid");

        if (invitation.ExpiresAt < DateTime.UtcNow)
            return JoinOrganizationResult.Failed(JoinOrganizationError.InvitationExpired, "This invitation has expired");

        var organization = invitation.Organization;

        if (organization is null)
            return JoinOrganizationResult.Failed(JoinOrganizationError.OrganizationNotFound, "Organization not found");

        if (organization.Members.Any(m => m.UserId == userId))
            return JoinOrganizationResult.Failed(JoinOrganizationError.AlreadyMember, "You are already a member of this organization");

        organization.Members.Add(new OrganizationMember
        {
            UserId = userId,
            OrganizationId = organization.Id,
            Role = invitation.Role,
            JoinedAt = DateTime.UtcNow,
        });

        invitation.Status = InvitationStatus.Accepted;

        await _organizationsRepository.UpdateAsync(organization);
        await _invitationsRepository.UpdateAsync(invitation);

        return JoinOrganizationResult.Succeeded(organization);
    }
}
