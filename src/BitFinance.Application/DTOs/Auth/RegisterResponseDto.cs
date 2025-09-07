using BitFinance.Application.DTOs.Users;

namespace BitFinance.Application.DTOs.Auth;

public class RegisterResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserDto User { get; set; } = null!;
}