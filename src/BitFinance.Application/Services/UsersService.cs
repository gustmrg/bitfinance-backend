using BitFinance.Application.Interfaces;
using BitFinance.Domain.Common;
using BitFinance.Domain.Common.Errors;
using BitFinance.Domain.Entities;
using BitFinance.Domain.Interfaces.Repositories;

namespace BitFinance.Application.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<Result<bool>> IsUserInOrganizationAsync(string userId, Guid organizationId)
    {
        var user = await _usersRepository.GetByIdAsync(userId);

        if (user is null)
            return IdentityErrors.UserNotFound;

        var isMember = user.Organizations.Any(o => o.Id == organizationId);
        return isMember;
    }

    public async Task<Result<User>> GetUserByIdAsync(string userId)
    {
        var user = await _usersRepository.GetByIdAsync(userId);

        if (user is null)
            return IdentityErrors.UserNotFound;

        return user;
    }

    public async Task<Result> UpdateUserAsync(User user)
    {
        await _usersRepository.UpdateAsync(user);
        return Result.Success();
    }
}
