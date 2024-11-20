using Api.Dtos.Products;
namespace Tests.Data;

public static class ProductsData
{
    public static CreateProductDto NewProduct => new(
        Name: "Test Product",
        Price: 99.99m,
        Description: "A product for testing.",
        StockQuantity: 10,
        ManufacturerId: Guid.NewGuid(),
        CategoryId: Guid.NewGuid(),
        ComponentCharacteristic: null);
}