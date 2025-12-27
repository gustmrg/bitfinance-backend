using BitFinance.Domain.Entities;

namespace BitFinance.Domain.Interfaces.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken, Guid>
{
    Task<RefreshToken?> GetByTokenAsync(string hashedToken);
    Task<List<RefreshToken>> GetActiveByUserIdAsync(string userId);
    Task RevokeAsync(RefreshToken refreshToken);
    Task RevokeAllForUserAsync(string userId);
}
