using BitFinance.Domain.Common;
using BitFinance.Domain.Entities;

namespace BitFinance.Application.Interfaces;

public interface IUsersService
{
    Task<Result<bool>> IsUserInOrganizationAsync(string userId, Guid organizationId);
    Task<Result<User>> GetUserByIdAsync(string userId);
    Task<Result> UpdateUserAsync(User user);
}
