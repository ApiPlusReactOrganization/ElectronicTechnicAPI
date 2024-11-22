using Domain.CartItems;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ICartItemRepository
{
    Task<Option<CartItem>> GetById(CartItemId id, CancellationToken cancellationToken);
    /*
    Task<Option<CartItem>> SearchByName(string name, CancellationToken cancellationToken);
    */
    Task<CartItem> Add(CartItem сategory, CancellationToken cancellationToken);
    Task<CartItem> Update(CartItem сategory, CancellationToken cancellationToken);
    Task<CartItem> Delete(CartItem сategory, CancellationToken cancellationToken);
}