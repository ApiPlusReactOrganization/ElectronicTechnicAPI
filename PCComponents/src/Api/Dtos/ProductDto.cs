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
    ManufacturerId ManufacturerId,
    CategoryId CategoryId,
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
            product.ManufacturerId,
            product.CategoryId,
            product.ComponentCharacteristic
        );
    }

    // => new(
    //         course.Id.Value,
    //         course.Name,
    //     );
}