using BitFinance.Application.Interfaces;
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

    public async Task<bool> IsUserInOrganizationAsync(string userId, Guid organizationId)
    {
        var user = await _usersRepository.GetByIdAsync(userId);

        if (user == null) return false;
        
        if (user.Organizations.All(o => o.Id != organizationId)) return false;

        return true;
    }

    public async Task<User?> GetUserByIdAsync(string userId)
    {
        return await _usersRepository.GetByIdAsync(userId);
    }

    public async Task UpdateUserAsync(User user)
    {
        await _usersRepository.UpdateAsync(user);
    }
}