using Application.Products.Exceptions;
using Domain.Products;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ProductErrorHandler
{
    public static ObjectResult ToObjectResult(this ProductException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                ProductCategoryNotFoundException
                    or ProductManufacturerNotFoundException
                    or ProductNotFoundException
                    or ProductInvalidCategoryException
                    or ImageNotFoundException
                    => StatusCodes.Status404NotFound,

                ProductNameExistsWithSameFieldsException 
                    or ProductCannotBeDeletedException
                    
                    => StatusCodes.Status409Conflict,

                ProductUnknownException or ImageSaveException 
                    => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Product error handler does not implemented")
            }
        };
    }
}