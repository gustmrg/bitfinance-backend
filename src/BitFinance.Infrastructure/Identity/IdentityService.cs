using BitFinance.Application.Interfaces;
using BitFinance.Domain.Common;
using BitFinance.Domain.Common.Errors;
using BitFinance.Domain.Entities;
using BitFinance.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BitFinance.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IUsersRepository _usersRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService,
        IUsersRepository usersRepository,
        IConfiguration configuration,
        ILogger<IdentityService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _usersRepository = usersRepository;
        _configuration = configuration;
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

        return CreateAuthenticationResult(user);
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

        return CreateAuthenticationResult(user);
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

        return Result.Success();
    }

    private AuthenticationResult CreateAuthenticationResult(User user)
    {
        var token = _tokenService.GenerateToken(user);
        var expiresAt = DateTime.UtcNow.AddMinutes(
            Convert.ToInt32(_configuration["Jwt:ExpirationInMinutes"]));

        return new AuthenticationResult(
            token,
            expiresAt,
            user.Id,
            user.Email ?? string.Empty,
            user.UserName ?? string.Empty);
    }
}
