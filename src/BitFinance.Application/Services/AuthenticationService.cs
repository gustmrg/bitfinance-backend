using BitFinance.Application.Common;
using BitFinance.Application.DTOs.Auth;
using BitFinance.Application.DTOs.Users;
using BitFinance.Application.Interfaces;
using BitFinance.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BitFinance.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

            if (!result.Succeeded)
            {
                var error = new Error(ErrorType.BusinessRule, "Invalid login attempt");
                return Result<LoginResponseDto>.Failure(error);
            }
        
            var user = await _userManager.FindByEmailAsync(request.Email);
            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        
            var tokenResponse = _tokenService.GenerateTokenResponse(userDto);

            var response = new LoginResponseDto
            {
                AccessToken = tokenResponse.Token,
                ExpiresAt = tokenResponse.ExpiresAt,
                User = userDto
            };
        
            return Result<LoginResponseDto>.Success(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result<bool>> LogoutAsync(string userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}