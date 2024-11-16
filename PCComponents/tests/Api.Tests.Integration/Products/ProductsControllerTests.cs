using System.Net;
using System.Net.Http.Json;
using Api.Dtos.Products;
using FluentAssertions;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Products;

public class ProductsControllerTests(IntegrationTestWebFactory factory) : BaseIntegrationTest(factory)
{
    // [Fact]
    // public async Task ShouldCreateProduct()
    // {
    //     // Arrange
    //     var request = ProductsData.NewProduct;
    //
    //     // Act
    //     var response = await Client.PostAsJsonAsync("products", request);
    //
    //     // Assert
    //     response.IsSuccessStatusCode.Should().BeTrue();
    // }

    [Fact]
    public async Task ShouldNotCreateProductWithoutName()
    {
        // Arrange
        var request = new CreateProductDto(
            Name: string.Empty,
            Price: 99.99m,
            Description: "A product for testing.",
            StockQuantity: 10,
            ManufacturerId: Guid.NewGuid(),
            CategoryId: Guid.NewGuid(),
            ComponentCharacteristic: null);

        // Act
        var response = await Client.PostAsJsonAsync("products", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}