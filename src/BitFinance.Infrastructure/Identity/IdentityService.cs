using BitFinance.Application.Interfaces;
using BitFinance.Application.Options;
using BitFinance.Domain.Common;
using BitFinance.Domain.Common.Errors;
using BitFinance.Domain.Entities;
using BitFinance.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BitFinance.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IUsersRepository _usersRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly JwtOptions _jwtOptions;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService,
        IUsersRepository usersRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IOptions<JwtOptions> jwtOptions,
        ILogger<IdentityService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _usersRepository = usersRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtOptions = jwtOptions.Value;
        _logger = logger;
    }

    public async Task<Result<AuthenticationResult>> RegisterAsync(
        string email,
        string password,
        string firstName,
        string lastName)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser is not null)
            return IdentityErrors.EmailAlreadyExists;

        var user = new User
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return IdentityErrors.RegistrationFailed(errors);
        }

        _logger.LogInformation("User registered: {Email}", email);

        await _signInManager.SignInAsync(user, isPersistent: false);

        return await CreateAuthenticationResultAsync(user);
    }

    public async Task<Result<AuthenticationResult>> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return IdentityErrors.InvalidCredentials;

        var result = await _signInManager.PasswordSignInAsync(
            user,
            password,
            isPersistent: false,
            lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            _logger.LogWarning("Account locked out: {Email}", email);
            return IdentityErrors.AccountLockedOut;
        }

        if (!result.Succeeded)
            return IdentityErrors.InvalidCredentials;

        _logger.LogInformation("User logged in: {Email}", email);

        return await CreateAuthenticationResultAsync(user);
    }

    public async Task<Result<User>> GetCurrentUserAsync(string userId)
    {
        var user = await _usersRepository.GetByIdAsync(userId);

        if (user is null)
            return IdentityErrors.UserNotFound;

        return user;
    }

    public async Task<Result<User>> UpdateProfileAsync(
        string userId,
        string firstName,
        string lastName)
    {
        var user = await _usersRepository.GetByIdAsync(userId);

        if (user is null)
            return IdentityErrors.UserNotFound;

        user.FirstName = firstName;
        user.LastName = lastName;

        await _usersRepository.UpdateAsync(user);

        return user;
    }

    public async Task<Result> ChangePasswordAsync(
        string userId,
        string currentPassword,
        string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return IdentityErrors.UserNotFound;

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return IdentityErrors.PasswordTooWeak(errors);
        }

        await _refreshTokenRepository.RevokeAllForUserAsync(userId);
        _logger.LogInformation("Password changed and all tokens revoked for user: {UserId}", userId);

        return Result.Success();
    }

    public async Task<Result<AuthenticationResult>> RefreshTokenAsync(string refreshToken)
    {
        var hashedToken = _tokenService.HashToken(refreshToken);
        var storedToken = await _refreshTokenRepository.GetByTokenAsync(hashedToken);

        if (storedToken is null || !storedToken.IsActive)
            return IdentityErrors.InvalidRefreshToken;

        var user = storedToken.User;

        await _refreshTokenRepository.RevokeAsync(storedToken);

        _logger.LogInformation("Token refreshed for user: {UserId}", user.Id);

        return await CreateAuthenticationResultAsync(user);
    }

    public async Task<Result> LogoutAsync(string refreshToken)
    {
        var hashedToken = _tokenService.HashToken(refreshToken);
        var storedToken = await _refreshTokenRepository.GetByTokenAsync(hashedToken);

        if (storedToken is not null && storedToken.IsActive)
        {
            await _refreshTokenRepository.RevokeAsync(storedToken);
            _logger.LogInformation("User logged out: {UserId}", storedToken.UserId);
        }

        return Result.Success();
    }

    private async Task<AuthenticationResult> CreateAuthenticationResultAsync(User user)
    {
        var accessToken = _tokenService.GenerateAccessToken(user);
        var accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes);

        var rawRefreshToken = _tokenService.GenerateRefreshToken();
        var hashedRefreshToken = _tokenService.HashToken(rawRefreshToken);
        var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationInDays);

        var refreshToken = new RefreshToken
        {
            Token = hashedRefreshToken,
            UserId = user.Id,
            ExpiresAt = refreshTokenExpiresAt
        };

        await _refreshTokenRepository.CreateAsync(refreshToken);

        return new AuthenticationResult(
            accessToken,
            accessTokenExpiresAt,
            rawRefreshToken,
            refreshTokenExpiresAt,
            user.Id,
            user.Email ?? string.Empty,
            user.FullName);
    }
}
