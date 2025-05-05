using BitFinance.Domain.Entities;

namespace BitFinance.Domain.Services;

public interface IUsersService
{
    Task<bool> IsUserInOrganizationAsync(string userId, Guid organizationId);
    Task<User?> GetUserByIdAsync(string userId);
    Task UpdateUserAsync(User user);
}