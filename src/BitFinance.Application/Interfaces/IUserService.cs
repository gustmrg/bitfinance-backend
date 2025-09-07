using BitFinance.Application.Common;
using BitFinance.Application.DTOs.Users;

namespace BitFinance.Application.Interfaces;

public interface IUserService
{
    Task<Result<UserDto>> CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<UserDto>> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result<UserDto>> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Result<UserDto>> UpdateUserAsync(string userId, UpdateUserRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> IsUserInOrganizationAsync(string userId, Guid organizationId);
}