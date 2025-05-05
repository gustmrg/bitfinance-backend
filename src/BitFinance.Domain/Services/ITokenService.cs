using BitFinance.Domain.Entities;

namespace BitFinance.Domain.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}