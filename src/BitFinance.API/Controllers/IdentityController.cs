using System.Security.Claims;
using Asp.Versioning;
using BitFinance.API.Extensions;
using BitFinance.Application.DTOs.Common;
using BitFinance.Application.DTOs.Identity;
using BitFinance.Application.DTOs.Organizations;
using BitFinance.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitFinance.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/identity")]
[Authorize]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<IdentityController> _logger;

    public IdentityController(
        IIdentityService identityService,
        ILogger<IdentityController> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _identityService.RegisterAsync(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName);

        if (result.IsSuccess)
            SetRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiresAt);

        return result.ToActionResult(auth => new
        {
            accessToken = auth.AccessToken,
            expiresIn = auth.ExpiresAt,
            user = new
            {
                id = auth.UserId,
                email = auth.Email,
                userName = auth.UserName
            }
        });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LogInAsync([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _identityService.LoginAsync(request.Email, request.Password);

        if (result.IsSuccess)
            SetRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiresAt);

        return result.ToActionResult(auth => new
        {
            accessToken = auth.AccessToken,
            expiresIn = auth.ExpiresAt,
            user = new
            {
                id = auth.UserId,
                email = auth.Email,
                userName = auth.UserName
            }
        });
    }
    
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return BadRequest("Invalid user");

        var result = await _identityService.GetCurrentUserAsync(userId);

        return result.ToActionResult(user =>
        {
            var organizations = user.Organizations
                .Select(o => new OrganizationSummary(o.Id, o.Name))
                .ToList();

            return new UserSummary(
                user.Id,
                user.FullName,
                user.Email ?? string.Empty,
                ReplaceUserName(user.UserName),
                organizations);
        });
    }
    
    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokenAsync()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized(new { message = "Refresh token not found" });

        var result = await _identityService.RefreshTokenAsync(refreshToken);

        if (result.IsSuccess)
            SetRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiresAt);

        return result.ToActionResult(auth => new
        {
            accessToken = auth.AccessToken,
            expiresIn = auth.ExpiresAt,
            user = new
            {
                id = auth.UserId,
                email = auth.Email,
                userName = auth.UserName
            }
        });
    }

    [HttpPost("manage/profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest model)
    {
        var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return BadRequest("Invalid user");

        var result = await _identityService.UpdateProfileAsync(
            userId,
            model.FirstName,
            model.LastName);

        return result.ToActionResult(user =>
        {
            var organizations = user.Organizations
                .Select(o => new OrganizationSummary(o.Id, o.Name))
                .ToList();

            return new UserSummary(
                user.Id,
                user.FullName,
                user.Email ?? string.Empty,
                ReplaceUserName(user.UserName),
                organizations);
        });
    }

    [HttpPost("manage/password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
    {
        var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return BadRequest("Invalid user");

        var result = await _identityService.ChangePasswordAsync(
            userId,
            request.CurrentPassword,
            request.NewPassword);

        return result.ToNoContentResult();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (!string.IsNullOrEmpty(refreshToken))
            await _identityService.LogoutAsync(refreshToken);

        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        return NoContent();
    }

    private static string ReplaceUserName(string? email)
        => string.IsNullOrEmpty(email) ? string.Empty : email.Split('@')[0];

    private void SetRefreshTokenCookie(string refreshToken, DateTime expiresAt)
    {
        Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = expiresAt
        });
    }
}
