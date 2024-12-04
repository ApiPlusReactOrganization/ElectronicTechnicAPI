using Domain.Authentications.Users;
using Domain.Orders;
using Domain.Products;

namespace Application.Orders.Exceptions;

public abstract class OrderException(OrderId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public OrderId OrderId { get; } = id;
}

public class OrderNotFoundException(OrderId id)
    : OrderException(id, $"Order item under id: {id} not found");

public class OrderAlreadyExistsException(OrderId id)
    : OrderException(id, $"Order item already exists: {id}");

public class OrderUnknownException(OrderId id, Exception innerException)
    : OrderException(id, $"Unknown exception for the cart item under id: {id}", innerException);

public class OrderHasRelatedEntitiesException(OrderId id)
    : OrderException(id, $"Order item with ID {id} cannot be deleted because it has related entities.");

public class OrderUserNotFoundException(UserId userId)
    : OrderException(OrderId.Empty, $"User under id '{userId}' not found");

public class StatusNotFoundException(string statusName)
    : OrderException(OrderId.Empty, $"Status under name '{statusName}' not found");

public class OrderUserCartIsEmpty(UserId userId)
    : OrderException(OrderId.Empty, $"User under id '{userId}' has no cart item.");

public class OrderQuantityExceedsStockException(ProductId productId, int stockQuantity)
    : OrderException(
        OrderId.Empty,
        $"Requested quantity exceeds stock for product {productId}. Available stock: {stockQuantity}.");