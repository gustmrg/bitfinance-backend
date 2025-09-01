using BitFinance.Infrastructure.Models;

namespace BitFinance.Application.Services.Interfaces;

public interface IUserSessionService
{
    Task<UserSession?> GetSessionAsync(string userId);
    Task SetCurrentOrganizationAsync(string userId, string organizationId);
    Task ClearSessionAsync(string userId);
}