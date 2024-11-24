using Domain.CartItems;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ICartItemQueries
{
    Task<IReadOnlyList<CartItem>> GetAll(CancellationToken cancellationToken);
    Task<Option<CartItem>> GetById(CartItemId id, CancellationToken cancellationToken);
}