using BitFinance.Application.Common;
using BitFinance.Application.DTOs.Auth;

namespace BitFinance.Application.Interfaces;

public interface IAuthenticationService
{
    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<bool>> LogoutAsync(string userId, CancellationToken cancellationToken = default);
}