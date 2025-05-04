using BitFinance.Business.Entities;

namespace BitFinance.Business.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}