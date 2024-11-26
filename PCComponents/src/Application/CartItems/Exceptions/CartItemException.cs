using Domain.CartItems;
using Domain.Carts;
using Domain.Products;

namespace Application.CartItems.Exceptions;

public abstract class CartItemException(CartItemId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public CartItemId CartItemId { get; } = id;
}

public class CartItemNotFoundException(CartItemId id)
    : CartItemException(id, $"Cart item under id: {id} not found");

public class CartItemAlreadyExistsException(CartItemId id)
    : CartItemException(id, $"Cart item already exists: {id}");

public class CartItemUnknownException(CartItemId id, Exception innerException)
    : CartItemException(id, $"Unknown exception for the cart item under id: {id}", innerException);

public class CartItemHasRelatedEntitiesException(CartItemId id)
    : CartItemException(id, $"Cart item with ID {id} cannot be deleted because it has related entities.");

public class CartItemCartNotFoundException(CartId cartId)
    : CartItemException(CartItemId.Empty, $"Cart under id '{cartId}' not found");

public class CartItemProductNotFoundException(ProductId productId)
    : CartItemException(CartItemId.Empty, $"Product under id '{productId}' not found");

public class CartItemQuantityExceedsStockException(ProductId productId, int stockQuantity)
    : CartItemException(
        CartItemId.Empty,
        $"Requested quantity exceeds stock for product {productId}. Available stock: {stockQuantity}.");