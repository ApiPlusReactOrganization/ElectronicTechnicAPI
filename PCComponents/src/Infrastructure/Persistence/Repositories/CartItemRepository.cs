using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Authentications.Users;
using Domain.CartItems;
using Domain.Products;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class CartItemRepository : ICartItemRepository, ICartItemQueries
{
    private readonly ApplicationDbContext _context;

    public CartItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<CartItem>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.CartItems
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<CartItem>> GetById(CartItemId id, CancellationToken cancellationToken)
    {
        var entity = await _context.CartItems
            .Include(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<CartItem>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<CartItem>> GetByUserId(UserId userId, CancellationToken cancellationToken)
    {
        return await _context.CartItems
            .Where(x => x.UserId == userId && x.IsFinished == false)
            .Include(x => x.User)
            .Include(x => x.Product)
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<CartItem>> GetByProduct(ProductId id, CancellationToken cancellationToken)
    {
        var entity = await _context.CartItems
            .FirstOrDefaultAsync(x => x.ProductId == id && x.IsFinished == false, cancellationToken);

        return entity == null ? Option.None<CartItem>() : Option.Some(entity);
    }

    public async Task<CartItem> Add(CartItem cartItem, CancellationToken cancellationToken)
    {
        await _context.CartItems.AddAsync(cartItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return cartItem;
    }

    public async Task<CartItem> Update(CartItem cartItem, CancellationToken cancellationToken)
    {
        _context.CartItems.Update(cartItem);
        await _context.SaveChangesAsync(cancellationToken);
        return cartItem;
    }

    public async Task<CartItem> Delete(CartItem cartItem, CancellationToken cancellationToken)
    {
        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync(cancellationToken);
        return cartItem;
    }
}