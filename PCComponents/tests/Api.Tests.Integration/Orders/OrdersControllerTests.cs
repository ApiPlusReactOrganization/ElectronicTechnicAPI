using System.Net.Http.Json;
using Api.Dtos;
using Domain.Authentications.Users;
using Domain.CartItems;
using Domain.Orders;
using Domain.Products;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Orders;

public class OrdersControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly User _mainUser = UsersData.MainUser;
    private readonly Order _mainOrder;
    private readonly Status _statusForUpdate = Status.New(StatusesConstants.Delivered);
    private readonly CartItem _mainCartItem;
    private readonly Product _mainProduct;


    public OrdersControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _mainProduct = ProductsData.MainProduct(Context.Manufacturers.First().Id, Context.Categories.First().Id);
        _mainOrder = OrdersData.MainOrder(_mainUser.Id);
        _mainCartItem = CartItemsData.MainCartItem(_mainUser.Id, _mainProduct.Id);

    }


    [Fact]
    public async Task ShouldCreateOrderWithCartItems()
    {
        // Arrange

        var request = new OrderDto(
            Id: null,
            UserId: _mainUser.Id.Value,
            TotalPrice: null,
            Status: StatusesConstants.Processing,
            CreatedAt: null,
            DeliveryAddress: "123 Main Street",
            CartItems: null
        );

        // Act
        var response = await Client.PostAsJsonAsync("orders/create", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var orderFromResponse = await response.Content.ReadFromJsonAsync<OrderDto>();
        orderFromResponse.Should().NotBeNull();
        orderFromResponse!.CartItems.Should().NotBeNull();
       
    }

    [Fact]
    public async Task ShouldFailToCreateOrderWithoutUserId()
    {
        // Arrange
        var request = new OrderDto(
            Id: null,
            UserId: null,
            TotalPrice: null,
            Status: StatusesConstants.Processing,
            CreatedAt: null,
            DeliveryAddress: "123 Main Street",
            CartItems: null
        );

        // Act
        var response = await Client.PostAsJsonAsync("orders/create", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task ShouldFailToCreateOrderWithoutDeliveryAddress()
    {
        // Arrange
        var request = new OrderDto(
            Id: null,
            UserId: _mainUser.Id.Value,
            TotalPrice: null,
            Status: StatusesConstants.Processing,
            CreatedAt: null,
            DeliveryAddress: null!,
            CartItems: null
        );

        // Act
        var response = await Client.PostAsJsonAsync("orders/create", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
    [Fact]
    public async Task ShouldFailToUpdateStatusForNonExistentOrder()
    {
        // Arrange
        var request = new StatusDto(Name: StatusesConstants.Delivered);
        var nonExistentOrderId = Guid.NewGuid();

        // Act
        var response = await Client.PutAsJsonAsync($"orders/update-status/{nonExistentOrderId}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task ShouldFailToUpdateOrderWithInvalidStatus()
    {
        // Arrange
        var request = new StatusDto(Name: "InvalidStatus");

        // Act
        var response = await Client.PutAsJsonAsync($"orders/update-status/{_mainOrder.Id.Value}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task ShouldReturnEmptyListWhenNoOrdersExist()
    {
        // Arrange
        Context.Orders.RemoveRange(Context.Orders);
        await SaveChangesAsync();

        // Act
        var response = await Client.GetAsync("orders/get-all");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var orders = await response.Content.ReadFromJsonAsync<List<OrderDto>>();
        orders.Should().NotBeNull();
        orders.Should().BeEmpty();
    }
    


    [Fact]
    public async Task ShouldGetAllOrders()
    {
        // Act
        var response = await Client.GetAsync("orders/get-all");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var orders = await response.Content.ReadFromJsonAsync<List<OrderDto>>();
        orders.Should().NotBeNull();
        orders.Should().Contain(x => x.Id == _mainOrder.Id.Value);
    }

    [Fact]
    public async Task ShouldUpdateOrderStatus()
    {
        // Arrange
        var request = new StatusDto(
           Name: StatusesConstants.Delivered
        );

        // Act
        var response = await Client.PutAsJsonAsync($"orders/update-status/{_mainOrder.Id.Value}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var updatedOrder = await response.Content.ReadFromJsonAsync<OrderDto>();
        updatedOrder.Should().NotBeNull();
        updatedOrder!.Status.Should().Be(_statusForUpdate.Name);

        var orderFromDatabase = await Context.Orders.FirstOrDefaultAsync(
            x => x.Id == _mainOrder.Id
        );

        orderFromDatabase.Should().NotBeNull();
        orderFromDatabase!.StatusId.Should().Be(_statusForUpdate.Id);
    }

    public async Task InitializeAsync()
    {
        await Context.Users.AddAsync(_mainUser);
        await Context.Products.AddAsync(_mainProduct);
        await Context.CartItems.AddAsync(_mainCartItem);
        await Context.Orders.AddAsync(_mainOrder);

        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Users.RemoveRange(Context.Users);
         Context.Products.RemoveRange(_mainProduct);
         Context.CartItems.RemoveRange(_mainCartItem);
         Context.Orders.RemoveRange(Context.Orders);

        await SaveChangesAsync();
    }
}
