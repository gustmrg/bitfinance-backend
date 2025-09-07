using BitFinance.Application.DTOs.Auth;
using BitFinance.Application.DTOs.Users;

namespace BitFinance.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(UserDto user);
    TokenResponse GenerateTokenResponse(UserDto user);
}