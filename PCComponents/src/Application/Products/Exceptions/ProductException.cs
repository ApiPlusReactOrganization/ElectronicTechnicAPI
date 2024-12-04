using Domain.Categories;
using Domain.Manufacturers;
using Domain.Products;

namespace Application.Products.Exceptions;

public abstract class ProductException(ProductId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public ProductId ProductId { get; } = id;
}

public class ProductNameExistsWithSameFieldsException(
    ProductId id,
    CategoryId categoryId,
    ManufacturerId manufacturerId)
    : ProductException(id,
        $"Product with the same name and other fields under " +
        $"category ID : {categoryId} and manufacturer ID : {manufacturerId} has already exists under ID : {id}");

public class ProductNotFoundException(ProductId id)
    : ProductException(id, $"Product under id: {id} not found");

public class ProductCategoryNotFoundException(CategoryId id)
    : ProductException(ProductId.Empty, $"Product Category under id: {id} not found");

public class ProductManufacturerNotFoundException(ManufacturerId id)
    : ProductException(ProductId.Empty, $"Product Manufacturer under id: {id} not found");

public class ProductUnknownException(ProductId id, Exception innerException)
    : ProductException(id, $"Unknown exception for the product under id: {id}", innerException);

public class ImageSaveException(ProductId id)
    : ProductException(id, $"Product under id: {id} have problems with images save!");

public class ImageNotFoundException(ProductImageId id)
    : ProductException(ProductId.Empty, $"Product image under id: {id} not found!");

public class ProductInvalidCategoryException(CategoryId id, string categoryName)
    : ProductException(
        ProductId.Empty,
        $"Selected characteristic does not match the category: {categoryName} ID: {id}");
        