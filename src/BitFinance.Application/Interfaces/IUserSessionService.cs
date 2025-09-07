using BitFinance.Application.Models;

namespace BitFinance.Application.Interfaces;

public interface IUserSessionService
{
    Task<UserSession?> GetSessionAsync(string userId);
    Task SetCurrentOrganizationAsync(string userId, Guid organizationId);
    Task ClearSessionAsync(string userId);
}