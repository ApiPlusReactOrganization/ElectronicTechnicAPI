using System.Net.Http.Json;
using Api.Dtos;
using Domain.Authentications.Users;
using Domain.CartItems;
using Domain.Categories;
using Domain.Manufacturers;
using Domain.Products;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.CartItems;

public class CartItemsControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly User _mainUser = UsersData.MainUser;
    private readonly Product _mainProduct;
    private readonly Product _productForCreate;
    private readonly CartItem _mainCartItem;

    public CartItemsControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _mainProduct = ProductsData.MainProduct(Context.Manufacturers.First().Id, Context.Categories.First().Id);
        _productForCreate = ProductsData.MainProduct(Context.Manufacturers.First().Id, Context.Categories.First().Id);
        
        _mainCartItem = CartItemsData.MainCartItem(_mainUser.Id, _mainProduct.Id);
    }
    
    [Fact]
    public async Task ShouldCreateCartItem()
    {
        // Arrange
        var request = new CartItemDto(
            Id: null,
            UserId: _mainUser.Id.Value,
            Quantity: 3,
            ProductId: _productForCreate.Id.Value,
            IsFinished: false,
            Product: null
        );

        // Act
        var response = await Client.PostAsJsonAsync("cart-items/create", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var cartItemFromResponse = await response.Content.ReadFromJsonAsync<CartItemDto>();
        cartItemFromResponse.Should().NotBeNull();
        cartItemFromResponse!.ProductId.Should().Be(_productForCreate.Id.Value);
        cartItemFromResponse.UserId.Should().Be(_mainUser.Id.Value);
        cartItemFromResponse.Quantity.Should().Be(3);

        var cartItemFromDatabase = await Context.CartItems.FirstOrDefaultAsync(
            x => x.Id == new CartItemId(cartItemFromResponse.Id!.Value)
        );

        cartItemFromDatabase.Should().NotBeNull();
        cartItemFromDatabase!.ProductId.Should().Be(_productForCreate.Id);
        cartItemFromDatabase.UserId.Should().Be(_mainUser.Id);
        cartItemFromDatabase.Quantity.Should().Be(3);
    }

    public async Task InitializeAsync()
    {
        await Context.Users.AddAsync(_mainUser);
        await Context.Products.AddAsync(_mainProduct);
        await Context.Products.AddAsync(_productForCreate);
        await Context.CartItems.AddAsync(_mainCartItem);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.CartItems.RemoveRange(Context.CartItems);
        Context.Products.RemoveRange(Context.Products);
        Context.Users.RemoveRange(Context.Users);
        await SaveChangesAsync();
    }
}
