using Application.Common.Interfaces.Repositories;
using Domain.RefreshTokens;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(ApplicationDbContext context) : IRefreshTokenRepository
{
    public async Task<Option<RefreshToken>> GetRefreshTokenAsync(string refreshToken,
        CancellationToken cancellationToken)
    {
        var entity = await context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == refreshToken, cancellationToken);
        
        return entity == null ? Option.None<RefreshToken>() : Option.Some(entity);
    }

    public async Task<RefreshToken> Create(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        await context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return refreshToken;
    }

    public async Task<RefreshToken> Update(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        context.RefreshTokens.Update(refreshToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return refreshToken;
    }
}