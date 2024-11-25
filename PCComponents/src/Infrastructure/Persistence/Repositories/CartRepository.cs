using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Carts;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class CartRepository : ICartRepository, ICartQueries
{
    private readonly ApplicationDbContext _context;

    public CartRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Cart>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.Carts
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Cart>> GetById(CartId id, CancellationToken cancellationToken)
    {
        var entity = await _context.Carts
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Cart>() : Option.Some(entity);
    }

    public async Task<Cart> Add(Cart cart, CancellationToken cancellationToken)
    {
        await _context.Carts.AddAsync(cart, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return cart;
    }

    public async Task<Cart> Update(Cart cart, CancellationToken cancellationToken)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync(cancellationToken);
        return cart;
    }

    public async Task<Cart> Delete(Cart cart, CancellationToken cancellationToken)
    {
        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync(cancellationToken);
        return cart;
    }
}