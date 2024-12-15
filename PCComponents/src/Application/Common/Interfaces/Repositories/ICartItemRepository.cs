using Domain.Authentications.Users;
using Domain.CartItems;
using Domain.Products;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ICartItemRepository
{
    Task<Option<CartItem>> GetById(CartItemId id, CancellationToken cancellationToken);
    Task<CartItem> Add(CartItem сategory, CancellationToken cancellationToken);
    Task<CartItem> Update(CartItem сategory, CancellationToken cancellationToken);
    Task<CartItem> Delete(CartItem сategory, CancellationToken cancellationToken);
    Task<Option<CartItem>> GetByProduct(ProductId id, UserId userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<CartItem>> GetByUserId(UserId userId, CancellationToken cancellationToken);
}