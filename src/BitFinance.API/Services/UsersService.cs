using BitFinance.API.Services.Interfaces;
using BitFinance.Data.Repositories.Interfaces;

namespace BitFinance.API.Services;

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
}