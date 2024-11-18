using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Api.Dtos.Products;
using Domain.Products.PCComponents;
using FluentAssertions;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Products;

public class ProductsControllerTests(IntegrationTestWebFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task ShouldCreateProduct()
    {
        // Arrange
        var categoriesResponse = await Client.GetAsync("categories");
        var manufacturersResponse = await Client.GetAsync("manufacturers");
        categoriesResponse.IsSuccessStatusCode.Should().BeTrue();
        manufacturersResponse.IsSuccessStatusCode.Should().BeTrue();

        var categories = await categoriesResponse.Content.ReadFromJsonAsync<List<CategoryDto>>();
        var manufacturers = await manufacturersResponse.Content.ReadFromJsonAsync<List<ManufacturerDto>>();

        categories.Should().NotBeNull();
        categories!.Should().NotBeEmpty();

        manufacturers.Should().NotBeNull();
        manufacturers!.Should().NotBeEmpty();

        var ramCategory = categories.FirstOrDefault(c => c.Name.Contains("RAM", StringComparison.OrdinalIgnoreCase));
        ramCategory.Should().NotBeNull("Category with 'RAM' should exist in the database.");
        var testManufacturer = manufacturers.FirstOrDefault(m =>
            m.Name.Contains("NVIDIA Corporation", StringComparison.OrdinalIgnoreCase));
        testManufacturer.Should().NotBeNull("Manufacturer with 'Test Manufacturer' should exist in the database.");
        var ram = new RAM
        {
            MemoryAmount = 16,
            MemorySpeed = 3200,
            MemoryType = "DDR4",
            FormFactor = "DIMM",
            Voltage = 1.35f,
            MemoryBandwidth = 25.6f
        };

        var componentCharacteristic = ComponentCharacteristic.NewRam(ram);

        var request = new CreateProductDto(
            Name: "Test RAM",
            Price: 99.99m,
            Description: "High-performance RAM for testing.",
            StockQuantity: 10,
            ManufacturerId: testManufacturer!.Id.Value,
            CategoryId: ramCategory!.Id.Value,
            ComponentCharacteristic: componentCharacteristic);

        // Act
        var response = await Client.PostAsJsonAsync("products", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var createdProduct = await response.Content.ReadFromJsonAsync<ProductDto>();
        createdProduct.Should().NotBeNull();
        createdProduct!.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task ShouldGetAllProducts()
    {
        // Act
        var response = await Client.GetAsync("products");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        products.Should().NotBeNull();
        products!.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ShouldGetProductById()
    {
        // Arrange
        var productsResponse = await Client.GetAsync("products");
        productsResponse.IsSuccessStatusCode.Should().BeTrue();
        var products = await productsResponse.Content.ReadFromJsonAsync<List<ProductDto>>();
        products.Should().NotBeNull();
        products!.Should().NotBeEmpty();

        var productId = products.First().Id!.Value;

        // Act
        var response = await Client.GetAsync($"products/{productId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        product.Should().NotBeNull();
        product!.Id.Should().Be(productId);
    }

    [Fact]
    public async Task ShouldGetProductsByCategory()
    {
        // Arrange
        var categoriesResponse = await Client.GetAsync("categories");
        categoriesResponse.IsSuccessStatusCode.Should().BeTrue();
        var categories = await categoriesResponse.Content.ReadFromJsonAsync<List<CategoryDto>>();
        categories.Should().NotBeNull();
        categories!.Should().NotBeEmpty();

        var categoryId = categories.First().Id!.Value;

        // Act
        var response = await Client.GetAsync($"products/under-category/{categoryId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        products.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldGetProductsByManufacturer()
    {
        // Arrange
        var manufacturersResponse = await Client.GetAsync("manufacturers");
        manufacturersResponse.IsSuccessStatusCode.Should().BeTrue();
        var manufacturers = await manufacturersResponse.Content.ReadFromJsonAsync<List<ManufacturerDto>>();
        manufacturers.Should().NotBeNull();
        manufacturers!.Should().NotBeEmpty();

        var manufacturerId = manufacturers.First().Id!.Value;

        // Act
        var response = await Client.GetAsync($"products/under-manufacturer/{manufacturerId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        products.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldGetProductsByCategoryAndManufacturer()
    {
        // Arrange
        var categoriesResponse = await Client.GetAsync("categories");
        var manufacturersResponse = await Client.GetAsync("manufacturers");

        categoriesResponse.IsSuccessStatusCode.Should().BeTrue();
        manufacturersResponse.IsSuccessStatusCode.Should().BeTrue();

        var categories = await categoriesResponse.Content.ReadFromJsonAsync<List<CategoryDto>>();
        var manufacturers = await manufacturersResponse.Content.ReadFromJsonAsync<List<ManufacturerDto>>();

        categories.Should().NotBeNull();
        categories!.Should().NotBeEmpty();

        manufacturers.Should().NotBeNull();
        manufacturers!.Should().NotBeEmpty();

        var categoryId = categories.First().Id!.Value;
        var manufacturerId = manufacturers.First().Id!.Value;

        // Act
        var response = await Client.GetAsync($"products/under-category-and-manufacturer/{categoryId}/{manufacturerId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        products.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldDeleteProduct()
    {
        // Arrange
        var categoriesResponse = await Client.GetAsync("categories");
        var manufacturersResponse = await Client.GetAsync("manufacturers");

        categoriesResponse.IsSuccessStatusCode.Should().BeTrue();
        manufacturersResponse.IsSuccessStatusCode.Should().BeTrue();

        var categories = await categoriesResponse.Content.ReadFromJsonAsync<List<CategoryDto>>();
        var manufacturers = await manufacturersResponse.Content.ReadFromJsonAsync<List<ManufacturerDto>>();

        categories.Should().NotBeNull();
        categories!.Should().NotBeEmpty();

        manufacturers.Should().NotBeNull();
        manufacturers!.Should().NotBeEmpty();

        var categoryId = categories.First().Id!.Value;
        var manufacturerId = manufacturers.First().Id!.Value;

        var ram = new RAM
        {
            MemoryAmount = 16,
            MemorySpeed = 3200,
            MemoryType = "DDR4",
            FormFactor = "DIMM",
            Voltage = 1.35f,
            MemoryBandwidth = 25.6f
        };

        var componentCharacteristic = ComponentCharacteristic.NewRam(ram);

        var request = new CreateProductDto(
            Name: "Product to Delete",
            Price: 49.99m,
            Description: "This product will be deleted.",
            StockQuantity: 5,
            ManufacturerId: manufacturerId,
            CategoryId: categoryId,
            ComponentCharacteristic: componentCharacteristic);
        
        var createResponse = await Client.PostAsJsonAsync("products", request);
        createResponse.IsSuccessStatusCode.Should().BeTrue();
        
        var getAllResponse = await Client.GetAsync("products");
        getAllResponse.IsSuccessStatusCode.Should().BeTrue();

        var products = await getAllResponse.Content.ReadFromJsonAsync<List<ProductDto>>();
        products.Should().NotBeNull();
        products!.Should().NotBeEmpty();

        var createdProduct = products.FirstOrDefault(p => p.Name == request.Name);
        createdProduct.Should().NotBeNull("The product should exist in the list of all products.");

        var productId = createdProduct!.Id!.Value;

        // Act: Delete the product
        var response = await Client.DeleteAsync($"products/{productId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
    }

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

    [Fact]
    public async Task ShouldNotCreateProductWithNegativePrice()
    {
        // Arrange
        var request = new CreateProductDto(
            Name: "Test Product",
            Price: -10m,
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

    [Fact]
    public async Task ShouldNotCreateProductWithInvalidStockQuantity()
    {
        // Arrange
        var request = new CreateProductDto(
            Name: "Test Product",
            Price: 99.99m,
            Description: "A product for testing.",
            StockQuantity: -5,
            ManufacturerId: Guid.NewGuid(),
            CategoryId: Guid.NewGuid(),
            ComponentCharacteristic: null);

        // Act
        var response = await Client.PostAsJsonAsync("products", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldNotCreateProductWithEmptyCategoryId()
    {
        // Arrange
        var request = new CreateProductDto(
            Name: "Test Product",
            Price: 99.99m,
            Description: "A product for testing.",
            StockQuantity: 10,
            ManufacturerId: Guid.NewGuid(),
            CategoryId: Guid.Empty,
            ComponentCharacteristic: null);

        // Act
        var response = await Client.PostAsJsonAsync("products", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldNotCreateProductWithEmptyManufacturerId()
    {
        // Arrange
        var request = new CreateProductDto(
            Name: "Test Product",
            Price: 99.99m,
            Description: "A product for testing.",
            StockQuantity: 10,
            ManufacturerId: Guid.Empty,
            CategoryId: Guid.NewGuid(),
            ComponentCharacteristic: null);

        // Act
        var response = await Client.PostAsJsonAsync("products", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldNotCreateProductWithInvalidDescription()
    {
        // Arrange
        var request = new CreateProductDto(
            Name: "Test Product",
            Price: 99.99m,
            Description: "No",
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
    [Fact]
    public async Task ShouldNotDeleteProductWithEmptyId()
    {
        // Arrange
        var invalidProductId = Guid.Empty;

        // Act
        var response = await Client.DeleteAsync($"products/{invalidProductId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldNotDeleteNonexistentProduct()
    {
        // Arrange
        var nonexistentProductId = Guid.NewGuid(); // ID that doesn't exist

        // Act
        var response = await Client.DeleteAsync($"products/{nonexistentProductId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

}