using Domain.Categories;
using Domain.Manufacturers;
using Domain.Products;

namespace Api.Dtos;

public record ProductDto
(
    Guid? Id,
    string Name,
    decimal Price ,
    string? Description,
    int StockQuantity,
    Guid? ManufacturerId,
    Guid? CategoryId,
    ComponentCharacteristic ComponentCharacteristic
    
)
{
    public static ProductDto FromDomainModel(Product product)
    {
        return new ProductDto(
            product.Id.Value,
            product.Name,
            product.Price,
            product.Description,
            product.StockQuantity,
            product.ManufacturerId.Value,
            product.CategoryId.Value,
            product.ComponentCharacteristic
        );
    }
}