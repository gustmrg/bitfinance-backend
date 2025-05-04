using System.Security.Claims;
using Asp.Versioning;
using BitFinance.API.InputModels;
using BitFinance.API.Models.Request;
using BitFinance.API.Services.Interfaces;
using BitFinance.API.ViewModels;
using BitFinance.Business.Entities;
using BitFinance.Business.Interfaces;
using BitFinance.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BitFinance.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/identity")]
public class IdentityController : ControllerBase
{
    private readonly IUsersService _usersService;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<IdentityController> _logger;
    private readonly IConfiguration _configuration;

    public IdentityController(IUsersService usersService, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<IdentityController> logger, ITokenService tokenService, IConfiguration configuration)
    {
        _usersService = usersService;
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _tokenService = tokenService;
        _configuration = configuration;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterInputModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = new User
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };
        
        var result = await _userManager.CreateAsync(user, model.Password);
        
        if (result.Succeeded)
        {
            _logger.LogInformation("User created a new account with password");
            
            await _signInManager.SignInAsync(user, isPersistent: false);
            
            var token = _tokenService.GenerateToken(user);
            
            return Ok(new
            {
                accessToken = token,
                expiresIn = DateTime.UtcNow.AddMinutes(
                    Convert.ToInt32(_configuration["Jwt:ExpirationInMinutes"])),
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    userName = user.UserName
                }
            });
        }
        
        return BadRequest(new { Errors = result.Errors });
    }

    [HttpPost("login")]
    public async Task<IActionResult> LogInAsync([FromBody] LoginInputModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var user = await _userManager.FindByEmailAsync(model.Email);
        
        if (user == null)
            return Unauthorized(new { message = "Invalid email or password" });
        
        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: true);
        
        if (result.Succeeded)
        {
            _logger.LogInformation("User logged in: {Email}", model.Email);
        
            var token = _tokenService.GenerateToken(user);
        
            return Ok(new
            {
                accessToken = token,
                expiresIn = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:ExpirationInMinutes"])),
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    userName = user.UserName
                }
            });
        }
    
        if (result.IsLockedOut)
        {
            _logger.LogWarning("User account locked out: {Email}", model.Email);
            return StatusCode(StatusCodes.Status403Forbidden, new { message = "Account locked out" });
        }
    
        return Unauthorized(new { message = "Invalid email or password" });
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return BadRequest("Invalid user");
            
        var user = await _usersService.GetUserByIdAsync(userId);
            
        if (user == null) return BadRequest("Invalid user");

        List<OrganizationViewModel> organizations = [];
        organizations.AddRange(user.Organizations.Select(organization => new OrganizationViewModel(organization.Id, organization.Name)));
        
        return Ok(new UserViewModel(user.Id, user.FullName, user?.Email ?? string.Empty, ReplaceUserName(user?.UserName), organizations));
    }

    [Authorize]
    [HttpPost("manage/profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileInputModel model)
    {
        var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return BadRequest("Invalid user");
            
        var user = await _usersService.GetUserByIdAsync(userId);
            
        if (user == null) return BadRequest("Invalid user");
        
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        
        await  _usersService.UpdateUserAsync(user);
        
        List<OrganizationViewModel> organizations = [];
        organizations.AddRange(user.Organizations.Select(organization => new OrganizationViewModel(organization.Id, organization.Name)));

        return Ok(new UserViewModel(user.Id, user.FullName, user.Email ?? string.Empty, ReplaceUserName(user?.UserName), organizations));
    }
    
    private static string ReplaceUserName(string? email)
    {
        return string.IsNullOrEmpty(email) ? string.Empty : email.Split('@')[0];
    }
}