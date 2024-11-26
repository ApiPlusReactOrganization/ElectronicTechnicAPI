using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.CartItems;
using Domain.Carts;
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
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<CartItem>() : Option.Some(entity);
    }
    
    public async Task<Option<CartItem>> GetByProduct(ProductId id, CancellationToken cancellationToken)
    {
        var entity = await _context.CartItems
            .FirstOrDefaultAsync(x => x.ProductId == id, cancellationToken);

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
