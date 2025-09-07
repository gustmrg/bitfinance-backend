using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BitFinance.Application.DTOs.Auth;
using BitFinance.Application.DTOs.Users;
using BitFinance.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BitFinance.Application.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public string GenerateToken(UserDto user)
    {
        var tokenResponse = GenerateTokenResponse(user);
        return tokenResponse.Token;
    }

    public TokenResponse GenerateTokenResponse(UserDto user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var expiresAt = DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration["Jwt:ExpirationInMinutes"]));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        return new TokenResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiresAt
        };
    }
}