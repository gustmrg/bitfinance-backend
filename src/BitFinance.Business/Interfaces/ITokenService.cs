using BitFinance.Business.Entities;

namespace BitFinance.Business.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);

    (string RawToken, RefreshToken Entity) GenerateRefreshToken(
        User user,
        Guid? tokenFamilyId = null,
        string? ipAddress = null,
        string? userAgent = null);

    Task<RefreshToken?> ValidateRefreshTokenAsync(string refreshToken);

    Task<(string AccessToken, string RefreshToken, RefreshToken Entity)?> RotateRefreshTokenAsync(
        RefreshToken currentToken,
        string? ipAddress = null,
        string? userAgent = null);

    string HashToken(string token);

    [Obsolete("Use GenerateAccessToken instead")]
    string GenerateToken(User user);
}