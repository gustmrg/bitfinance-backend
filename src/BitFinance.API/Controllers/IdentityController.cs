using System.Security.Claims;
using BitFinance.API.Extensions;
using BitFinance.Application.Common;
using BitFinance.Application.DTOs.Auth;
using BitFinance.Application.DTOs.Users;
using BitFinance.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitFinance.API.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<IdentityController> _logger;

    public IdentityController(
        IUserService userService, 
        IAuthenticationService authService,
        ITokenService tokenService,
        ILogger<IdentityController> logger)
    {
        _userService = userService;
        _authService = authService;
        _tokenService = tokenService;
        _logger = logger;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(
        [FromBody] RegisterRequestDto request, 
        CancellationToken cancellationToken = default)
    {
        var createUserRequest = new CreateUserRequestDto
        {
            Email = request.Email,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var userResult = await _userService.CreateUserAsync(createUserRequest, cancellationToken);

        if (!userResult.IsSuccess)
        {
            return userResult.ToActionResult();
        }

        var user = userResult.Data!; 
        
        _logger.LogInformation("User {Id} registered successfully", user.Id);
        
        var loginRequest = new LoginRequestDto
        {
            Email = user.Email,
            Password = request.Password
        };
        
        var signInResult = await _authService.LoginAsync(loginRequest, cancellationToken);
        
        if (!signInResult.IsSuccess)
        {
            _logger.LogWarning("Failed to sign in newly registered user {UserId}", user.Id);
            return signInResult.ToActionResult();
        }

        var tokenResponse = _tokenService.GenerateTokenResponse(user);

        var response = new RegisterResponseDto
        {
            AccessToken = tokenResponse.Token,
            ExpiresAt = tokenResponse.ExpiresAt,
            User = user
        };
        
        var result = Result<RegisterResponseDto>.Success(response);
        return result.ToActionResult();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LogInAsync(
        [FromBody] LoginRequestDto request, 
        CancellationToken cancellationToken = default)
    {
        var signInResult = await _authService.LoginAsync(request, cancellationToken);
        
        if (!signInResult.IsSuccess)
        {
            _logger.LogWarning("Failed to sign in user with email {Email}", request.Email);
        }
        
        return signInResult.ToActionResult();
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = User.GetUserId();
        
        if (string.IsNullOrEmpty(userId))
        {
            return Result<UserDto>.Failure(DomainErrors.Users.NotFound).ToActionResult();
        }
            
        var userResult = await _userService.GetUserByIdAsync(userId);
            
        if (!userResult.IsSuccess)
        {
            return userResult.ToActionResult();
        }

        // List<OrganizationViewModel> organizations = [];
        // organizations.AddRange(user.Organizations.Select(organization => new OrganizationViewModel(organization.Id, organization.Name)));
        // return Ok(new UserViewModel(user.Id, user.FullName, user?.Email ?? string.Empty, ReplaceUserName(user?.UserName), organizations));
        
        var result = Result<UserDto>.Success(userResult.Data!);
        return result.ToActionResult();
    }

    // DISABLED
    /*
    [Authorize]
    [HttpPost("manage/profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileInputModel model)
    {
        var userId = User.GetUserId();
        
        if (string.IsNullOrEmpty(userId)) return BadRequest("Invalid user");
            
        var user = await _userService.GetUserByIdAsync(userId);
            
        if (user == null) return BadRequest("Invalid user");
        
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        
        await  _userService.UpdateUserAsync(user);
        
        List<OrganizationViewModel> organizations = [];
        organizations.AddRange(user.Organizations.Select(organization => new OrganizationViewModel(organization.Id, organization.Name)));

        return Ok(new UserViewModel(user.Id, user.FullName, user.Email ?? string.Empty, ReplaceUserName(user?.UserName), organizations));
    }
    */
    
    private static string ReplaceUserName(string? email)
    {
        return string.IsNullOrEmpty(email) ? string.Empty : email.Split('@')[0];
    }
}