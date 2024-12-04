using Api.Dtos.Products;
using Domain.Categories;
using Domain.Manufacturers;
using Domain.Products;
using Domain.Products.PCComponents;

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

    public static Product MainProduct(ManufacturerId manufacturerId, CategoryId categoryId) => Product.New(
        id: ProductId.New(),
        name: "Main Test Product",
        price: 100.0m,
        description: "Main Test description",
        stockQuantity: 10,
        manufacturerId: manufacturerId,
        categoryId: categoryId,
        componentCharacteristic: ComponentCharacteristic.NewRam(new RAM
        {
            MemoryAmount = 16,
            MemorySpeed = 3200,
            MemoryType = "DDR4",
            FormFactor = "DIMM",
            Voltage = 1.35f,
            MemoryBandwidth = 25.6f
        })
    );
}