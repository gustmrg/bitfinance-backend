using BitFinance.Domain.Entities;

namespace BitFinance.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}