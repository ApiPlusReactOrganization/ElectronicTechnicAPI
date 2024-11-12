using Application.Manufacturers.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ManufacturerErrorHandler
{
    public static ObjectResult ToObjectResult(this ManufacturerException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                ManufacturerNotFoundException => StatusCodes.Status404NotFound,
                ManufacturerAlreadyExistsException or ManufacturerHasRelatedProductsException 
                    => StatusCodes.Status409Conflict,
                ManufacturerUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Manufacturer error handler does not implemented")
            }
        };
    }
}