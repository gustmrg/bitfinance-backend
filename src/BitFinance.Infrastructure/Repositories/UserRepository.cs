using BitFinance.Domain.Entities;
using BitFinance.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BitFinance.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<User> CreateAsync(User user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        await _userManager.UpdateAsync(user);
    }
}