using Domain.Products;
using Domain.Products.PCComponents;

namespace Api.Dtos.Products;

public record CreateProductDto(
    string Name,
    decimal Price,
    string? Description,
    int StockQuantity,
    Guid ManufacturerId,
    Guid CategoryId,
    ComponentCharacteristic ComponentCharacteristic
)
{
    public static CreateProductDto FromDomainModel(Product product)
    {
        return new CreateProductDto(
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