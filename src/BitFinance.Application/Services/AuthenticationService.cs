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
            // First check if user exists
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result<LoginResponseDto>.Failure(DomainErrors.Authentication.UserNotFound);
            }

            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

            if (!result.Succeeded)
            {
                // Provide specific error messages based on SignInResult
                if (result.IsLockedOut)
                {
                    return Result<LoginResponseDto>.Failure(DomainErrors.Authentication.AccountLocked);
                }
                
                if (result.IsNotAllowed)
                {
                    return Result<LoginResponseDto>.Failure(DomainErrors.Authentication.AccountNotConfirmed);
                }
                
                if (result.RequiresTwoFactor)
                {
                    return Result<LoginResponseDto>.Failure(Error.BusinessRule(
                        "Authentication.RequiresTwoFactor", 
                        "Two-factor authentication is required for this account."));
                }

                // For invalid password or other failures
                return Result<LoginResponseDto>.Failure(DomainErrors.Authentication.InvalidCredentials);
            }

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
        catch (Exception ex)
        {
            return Result<LoginResponseDto>.Failure(Error.Infrastructure(
                "Authentication.LoginError", 
                "An error occurred during login. Please try again."));
        }
    }

    public async Task<Result<bool>> LogoutAsync(string userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}