using System.Linq.Expressions;
using BitFinance.Domain.Entities;
using BitFinance.Domain.Interfaces.Repositories;
using BitFinance.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BitFinance.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RefreshTokenRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<RefreshToken>> GetAllAsync()
    {
        return await _dbContext.Set<RefreshToken>().ToListAsync();
    }

    public async Task<RefreshToken?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<RefreshToken>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<RefreshToken?> GetByTokenAsync(string hashedToken)
    {
        return await _dbContext.Set<RefreshToken>()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == hashedToken);
    }

    public async Task<List<RefreshToken>> GetActiveByUserIdAsync(string userId)
    {
        return await _dbContext.Set<RefreshToken>()
            .Where(x => x.UserId == userId && x.RevokedAt == null && x.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task<RefreshToken> CreateAsync(RefreshToken entity)
    {
        _dbContext.Set<RefreshToken>().Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(RefreshToken entity)
    {
        _dbContext.Set<RefreshToken>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(RefreshToken entity, params Expression<Func<RefreshToken, object>>[] properties)
    {
        var entry = _dbContext.Entry(entity);
        foreach (var property in properties)
        {
            entry.Property(property).IsModified = true;
        }
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(RefreshToken entity)
    {
        _dbContext.Set<RefreshToken>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RevokeAsync(RefreshToken refreshToken)
    {
        refreshToken.RevokedAt = DateTime.UtcNow;
        await UpdateAsync(refreshToken);
    }

    public async Task RevokeAllForUserAsync(string userId)
    {
        var activeTokens = await GetActiveByUserIdAsync(userId);
        foreach (var token in activeTokens)
        {
            token.RevokedAt = DateTime.UtcNow;
        }
        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
