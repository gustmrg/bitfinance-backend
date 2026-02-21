using BitFinance.API.Models;
using BitFinance.Business.Entities;
using BitFinance.Business.Enums;

namespace BitFinance.API.Services.Interfaces;

/// <summary>
/// Provides operations for managing organization invitations.
/// </summary>
public interface IInvitationsService
{
    /// <summary>
    /// Creates a new invitation for a user to join an organization.
    /// </summary>
    /// <param name="organizationId">The organization's ID.</param>
    /// <param name="email">The email address of the invited user.</param>
    /// <param name="role">The role the invited user will have in the organization.</param>
    /// <param name="invitedByUserId">The ID of the user creating the invitation.</param>
    /// <returns>The created <see cref="Invitation"/> entity.</returns>
    Task<Invitation> CreateInvitationAsync(Guid organizationId, string email, OrgRole role, string invitedByUserId);

    /// <summary>
    /// Processes accepting an invitation: validates the token, checks expiry and status,
    /// prevents duplicate membership, adds the user as a member, and marks the invitation accepted.
    /// </summary>
    /// <param name="token">The invitation token.</param>
    /// <param name="userId">The ID of the user accepting the invitation.</param>
    /// <returns>A <see cref="JoinOrganizationResult"/> indicating success or the specific failure reason.</returns>
    Task<JoinOrganizationResult> JoinOrganizationAsync(string token, string userId);
}
