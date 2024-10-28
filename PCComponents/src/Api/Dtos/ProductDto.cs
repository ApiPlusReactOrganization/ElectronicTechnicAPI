using Domain.Categories;
using Domain.ComponentCharacteristics;
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
    Case Case
    
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
            product.ComponentCharacteristic.Case
        );
    }

    // => new(
    //         course.Id.Value,
    //         course.Name,
    //     );
}