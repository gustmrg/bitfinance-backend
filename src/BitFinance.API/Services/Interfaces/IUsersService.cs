using BitFinance.Business.Entities;

namespace BitFinance.API.Services.Interfaces;

public interface IUsersService
{
    Task<bool> IsUserInOrganizationAsync(string userId, Guid organizationId);
    Task<User?> GetUserByIdAsync(string userId);
    Task UpdateUserAsync(User user);
}