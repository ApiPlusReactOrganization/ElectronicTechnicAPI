using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Api.Dtos.Authentications;
using Api.Dtos.Products;
using Api.Dtos.Users;
using Application.ViewModels;
using Domain.Products.PCComponents;
using FluentAssertions;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Users;

public class UsersControllerTests(IntegrationTestWebFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task ShouldGetAllUsers()
    {
        // Arrange
        var signUpRequest = UsersData.SignUpMainUser;
        var signUpResponse = await Client.PostAsJsonAsync("account/signup", signUpRequest);
        signUpResponse.IsSuccessStatusCode.Should().BeTrue("Користувач має успішно зареєструватись");

        // Act
        var usersResponse = await Client.GetFromJsonAsync<UserDto[]>("users/get-all");

        // Assert
        usersResponse.Should().NotBeNullOrEmpty();
        usersResponse!.Any(u => u.Email == signUpRequest.Email).Should().BeTrue("Користувач має бути у списку");
    }

    [Fact]
    public async Task ShouldDeleteUser()
    {
        // Arrange
        var signUpRequest = UsersData.SignUpDeleteUser;
        var signUpResponse = await Client.PostAsJsonAsync("account/signup", signUpRequest);
        signUpResponse.IsSuccessStatusCode.Should().BeTrue("Користувач має успішно зареєструватись");


        var users = await Client.GetFromJsonAsync<UserDto[]>("users/get-all");
        var mainUser = users!.FirstOrDefault(u => u.Email == signUpRequest.Email);
        mainUser.Should().NotBeNull("Користувач має існувати");

        // Act
        var deleteResponse = await Client.DeleteAsync($"users/delete/{mainUser!.Id}");

        // Assert
        deleteResponse.IsSuccessStatusCode.Should().BeTrue("Користувач має бути успішно видалений");
    }

    [Fact]
    public async Task ShouldUpdateUserRoles()
    {
        // Arrange
        var signUpRequest = UsersData.SignUpUpdateUserRoles;
        var signUpResponse = await Client.PostAsJsonAsync("account/signup", signUpRequest);
        signUpResponse.IsSuccessStatusCode.Should().BeTrue("Користувач має успішно зареєструватись");


        var users = await Client.GetFromJsonAsync<UserDto[]>("users/get-all");
        var mainUser = users!.FirstOrDefault(u => u.Email == signUpRequest.Email);
        mainUser.Should().NotBeNull("Користувач має існувати");

 
        var newRoles = new List<RoleDto>
        {
            new("Administrator")
        };

        // Act
        var updateRolesResponse = await Client.PutAsJsonAsync($"users/update-roles/{mainUser!.Id}", newRoles);

        // Assert
        updateRolesResponse.IsSuccessStatusCode.Should().BeTrue("Ролі мають бути успішно оновлені");
    }

    [Fact]
    public async Task ShouldUploadUserImage()
    {
        // Arrange
        var signUpRequest = UsersData.SignUpUploadUser;
        var signUpResponse = await Client.PostAsJsonAsync("account/signup", signUpRequest);
        signUpResponse.IsSuccessStatusCode.Should().BeTrue("Користувач має успішно зареєструватись");


        var users = await Client.GetFromJsonAsync<UserDto[]>("users/get-all");
        var mainUser = users!.FirstOrDefault(u => u.Email == signUpRequest.Email);
        mainUser.Should().NotBeNull("Користувач має існувати");
        
        using var imageContent = new ByteArrayContent(new byte[0]);
        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        var content = new MultipartFormDataContent
        {
            { imageContent, "imageFile", "test.png" }
        };

        // Act
        var uploadImageResponse = await Client.PutAsync($"users/image/{mainUser!.Id}", content);

        // Assert
        uploadImageResponse.IsSuccessStatusCode.Should().BeTrue("Зображення має бути успішно завантажене");
    }


        [Fact]
    public async Task ShouldNotDeleteNonExistingUser()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"users/delete/{userId}");

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound, "Користувача не існує");
    }

    [Fact]
    public async Task ShouldNotUpdateRolesForNonExistingUser()
    {
        // Arrange
        var nonExistingUserId = Guid.NewGuid();
        var newRoles = new List<RoleDto>
        {
            new("Administrator")
        };

        // Act
        var response = await Client.PutAsJsonAsync($"users/update-roles/{nonExistingUserId}", newRoles);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound, "Користувач не існує");
    }

    [Fact]
    public async Task ShouldNotUpdateRolesWithInvalidRoles()
    {
        // Arrange
        var signUpRequest = UsersData.SignUpNotUpdateUserRoles;
        var signUpResponse = await Client.PostAsJsonAsync("account/signup", signUpRequest);
        signUpResponse.IsSuccessStatusCode.Should().BeTrue("Користувач має успішно зареєструватись");

        // Отримуємо ID користувача
        var users = await Client.GetFromJsonAsync<UserDto[]>("users/get-all");
        var mainUser = users!.FirstOrDefault(u => u.Email == signUpRequest.Email);
        mainUser.Should().NotBeNull("Користувач має існувати");

        // Порожній список ролей
        var emptyRoles = new List<RoleDto>();

        // Act
        var updateRolesResponse = await Client.PutAsJsonAsync($"users/update-roles/{mainUser!.Id}", emptyRoles);

        // Assert
        updateRolesResponse.IsSuccessStatusCode.Should().BeFalse();
        updateRolesResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, "Ролі не можуть бути порожніми");
    }

    [Fact]
    public async Task ShouldNotUploadImageForNonExistingUser()
    {
        // Arrange
        var nonExistingUserId = Guid.NewGuid();

        using var imageContent = new ByteArrayContent(new byte[0]);
        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        var content = new MultipartFormDataContent
        {
            { imageContent, "imageFile", "test.png" }
        };

        // Act
        var response = await Client.PutAsync($"users/Image/{nonExistingUserId}", content);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound, "Користувач не існує");
    }
    
    [Fact]
    public async Task ShouldNotRegisterUserWithInvalidData()
    {
        // Arrange
        var invalidSignUpRequest = UsersData.invalidSignUpRequest;
        // Act
        var signUpResponse = await Client.PostAsJsonAsync("account/signup", invalidSignUpRequest);

        // Assert
        signUpResponse.IsSuccessStatusCode.Should().BeFalse();
        signUpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, "Запит має бути некоректним");
    }
    
       [Fact]
    public async Task ShouldAddFavoriteProduct()
    {
        // Arrange
        var signUpRequest = UsersData.SignUpMainUser;
        var signUpResponse = await Client.PostAsJsonAsync("account/signup", signUpRequest);
        signUpResponse.IsSuccessStatusCode.Should().BeTrue();

        var users = await Client.GetFromJsonAsync<UserDto[]>("users/get-all");
        var user = users!.FirstOrDefault(u => u.Email == signUpRequest.Email);
        user.Should().NotBeNull("Користувач має існувати");

        var productsResponse = await Client.GetAsync("products/get-all");
        productsResponse.IsSuccessStatusCode.Should().BeTrue();
        productsResponse.Should().NotBeNull();
        var products = await productsResponse.Content.ReadFromJsonAsync<List<CategoryDto>>();
        var product = products.FirstOrDefault();
        product.Should().NotBeNull();


        // Act
        var response = await Client.PutAsJsonAsync<object>($"users/{user.Id}/favorite-products-add/{product.Id}", null);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = await response.Content.ReadFromJsonAsync<UserFavoriteProductsDto>();
        content.Should().NotBeNull();
        content!.FavoriteProducts.Should().ContainSingle(p => p.Id == product.Id);
    }

    [Fact]
    public async Task ShouldNotAddFavoriteProductToNonExistentUser()
    {
        // Arrange
        var userId = Guid.NewGuid(); // Non-existent User ID
        var productId = Guid.NewGuid();

        // Act
        var response = await Client.PutAsJsonAsync<object>($"users/{userId}/favorite-products-add/{productId}", null);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldRemoveFavoriteProduct()
    {
        // Arrange
        var signUpRequest = UsersData.RemoveFavorite;
        var signUpResponse = await Client.PostAsJsonAsync("account/signup", signUpRequest);
        signUpResponse.IsSuccessStatusCode.Should().BeTrue();

        var users = await Client.GetFromJsonAsync<UserDto[]>("users/get-all");
        var user = users!.FirstOrDefault(u => u.Email == signUpRequest.Email);
        user.Should().NotBeNull("Користувач має існувати");

        
        
        
        var categoriesResponse = await Client.GetAsync("categories/get-all");
        var manufacturersResponse = await Client.GetAsync("manufacturers/get-all");
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
            Name: "RemoveFavorite Test RAM",
            Price: 99.99m,
            Description: "RemoveFavorite High-performance RAM for testing.",
            StockQuantity: 10,
            ManufacturerId: testManufacturer!.Id.Value,
            CategoryId: ramCategory!.Id.Value,
            ComponentCharacteristic: componentCharacteristic);

        var productResponse = await Client.PostAsJsonAsync("products/create", request);
        productResponse.IsSuccessStatusCode.Should().BeTrue();
        productResponse.Should().NotBeNull();
        
        var productsResponse = await Client.GetAsync("products/get-all");
        productsResponse.IsSuccessStatusCode.Should().BeTrue();
        productsResponse.Should().NotBeNull();
        var products = await productsResponse.Content.ReadFromJsonAsync<List<CategoryDto>>();
        var product = products.FirstOrDefault(c => c.Name == "RemoveFavorite Test RAM");
        product.Should().NotBeNull();


        
        var addResponse = await Client.PutAsJsonAsync<object>($"users/{user.Id}/favorite-products-add/{product.Id}", null);
        addResponse.IsSuccessStatusCode.Should().BeTrue();

        // Act
        var removeResponse = await Client.PutAsJsonAsync<object>($"users/{user.Id}/favorite-products-remove/{product.Id}", null);

        // Assert
        removeResponse.IsSuccessStatusCode.Should().BeTrue();
        var content = await removeResponse.Content.ReadFromJsonAsync<UserFavoriteProductsDto>();
        content.Should().NotBeNull();
        content!.FavoriteProducts.Should().NotContain(p => p.Id == product.Id);
    }
    

}