using BitFinance.Application.Common;
using BitFinance.Application.DTOs.Users;
using BitFinance.Application.Interfaces;
using BitFinance.Domain.Entities;
using BitFinance.Domain.Interfaces;

namespace BitFinance.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Result<UserDto>> CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
            };
            
            await _userRepository.CreateAsync(user, request.Password);

            var data = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName ?? string.Empty,
            };

            var result = Result<UserDto>.Success(data);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result<UserDto>> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var user =  await _userRepository.GetByIdAsync(userId);
            
            if (user is null)
                return Result<UserDto>.NotFound("User not found");
            
            var data = new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName ?? string.Empty,
            };

            var result = Result<UserDto>.Success(data);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result<UserDto>> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var user =  await _userRepository.GetByEmailAsync(email);
            
            if (user is null)
                return Result<UserDto>.NotFound("User not found");
            
            var data = new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName ?? string.Empty,
            };

            var result = Result<UserDto>.Success(data);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result<UserDto>> UpdateUserAsync(string userId, UpdateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<bool>> DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsUserInOrganizationAsync(string userId, Guid organizationId)
    {
        var user =  await _userRepository.GetByIdAsync(userId);
        
        if (user is null) 
            return false;
        
        if (user.Organizations.All(o => o.Id != organizationId)) 
            return false;

        return true;
    }
}