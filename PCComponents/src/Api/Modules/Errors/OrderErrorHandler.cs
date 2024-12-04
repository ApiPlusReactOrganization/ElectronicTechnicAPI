using Application.Orders.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class OrderErrorHandler
{
    public static ObjectResult ToObjectResult(this OrderException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                OrderNotFoundException 
                    or OrderUserCartIsEmpty 
                    or OrderUserNotFoundException 
                    or StatusNotFoundException =>
                    StatusCodes.Status404NotFound,
                OrderAlreadyExistsException or OrderHasRelatedEntitiesException
                    or OrderQuantityExceedsStockException
                    => StatusCodes.Status409Conflict,
                OrderUnknownException =>
                    StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Order error handler does not implemented")
            }
        };
    }
}