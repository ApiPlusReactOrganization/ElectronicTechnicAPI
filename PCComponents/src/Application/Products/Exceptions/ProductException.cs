using Domain.Categories;
using Domain.Manufacturers;
using Domain.Products;

namespace Application.Products.Exceptions;


public abstract class ProductException(ProductId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public ProductId ProductId { get; } = id;
}
public class ProductUnderCurrentCategoryAlreadyExistsException(CategoryId categoryId) 
    : ProductException( ProductId.Empty,  $"Product under category with id: {categoryId} already exists: ");

public class ProductNotFoundException(ProductId id) 
    : ProductException(id, $"Product under id: {id} not found");

public class ProductCategoryNotFoundException(CategoryId id) 
    : ProductException(ProductId.Empty, $"Product Category under id: {id} not found");

public class ProductManufacturerNotFoundException(ManufacturerId id) 
    : ProductException(ProductId.Empty, $"Product Manufacturer under id: {id} not found");

public class ProductUnknownException(ProductId id, Exception innerException)
    : ProductException(id, $"Unknown exception for the product under id: {id}", innerException);