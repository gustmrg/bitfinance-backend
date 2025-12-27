using BitFinance.Domain.Common;
using BitFinance.Domain.Entities;

namespace BitFinance.Application.Interfaces;

public interface IIdentityService
{
    Task<Result<AuthenticationResult>> RegisterAsync(
        string email,
        string password,
        string firstName,
        string lastName);

    Task<Result<AuthenticationResult>> LoginAsync(
        string email,
        string password);

    Task<Result<User>> GetCurrentUserAsync(string userId);

    Task<Result<User>> UpdateProfileAsync(
        string userId,
        string firstName,
        string lastName);

    Task<Result> ChangePasswordAsync(
        string userId,
        string currentPassword,
        string newPassword);
}

public record AuthenticationResult(
    string AccessToken,
    DateTime ExpiresAt,
    string UserId,
    string Email,
    string UserName);
