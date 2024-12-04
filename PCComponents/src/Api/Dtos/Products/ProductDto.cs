using Domain.Products;
using Domain.Products.PCComponents;

namespace Api.Dtos.Products;

public record ProductDto(
    Guid? Id,
    string Name,
    decimal Price,
    string Description,
    int StockQuantity,
    ManufacturerDto? Manufacturer,
    CategoryDto? Category,
    List<ProductImageDto>? Images,
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
            product.Manufacturer == null ? null : ManufacturerDto.FromDomainModel(product.Manufacturer),
            product.Category == null ? null : CategoryDto.FromDomainModel(product.Category),
            product.Images.Select(ProductImageDto.FromDomainModel).ToList(),
            product.ComponentCharacteristic
        );
    }
}