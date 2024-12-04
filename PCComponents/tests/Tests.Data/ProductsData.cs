using Api.Dtos.Products;
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
        ComponentCharacteristic: new ComponentCharacteristic
        {
            Case = new Case
            {
                NumberOfFans = 2,
                CoolingDescription = "Efficient cooling system.",
                FormFactor = "Mid Tower",
                CompartmentDescription = "Standard ATX Compartment",
                PortsDescription = "USB 3.0, USB-C, Audio Jack"
            }
        });
}