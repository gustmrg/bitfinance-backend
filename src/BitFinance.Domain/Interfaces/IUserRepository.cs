using BitFinance.Domain.Entities;

namespace BitFinance.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user, string password);
    Task UpdateAsync(User user);
}