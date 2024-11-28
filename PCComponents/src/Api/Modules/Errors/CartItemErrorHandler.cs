using Application.CartItems.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class CartItemErrorHandler
{
    public static ObjectResult ToObjectResult(this CartItemException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                CartItemNotFoundException or ProductForCartItemNotFoundException =>
                    StatusCodes.Status404NotFound,
                CartItemAlreadyExistsException or CartItemHasRelatedEntitiesException
                    or CartItemQuantityExceedsStockException
                    or CartItemHaveOrderException
                    => StatusCodes.Status409Conflict,
                CartItemUnknownException =>
                    StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Cart item error handler does not implemented")
            }
        };
    }
}