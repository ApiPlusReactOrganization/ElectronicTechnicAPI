using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Manufacturers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Manufacturers;

public class ManufacturersControllerTests(IntegrationTestWebFactory factory)
    : BaseIntegrationTest(factory), IAsyncLifetime
{
    private readonly Manufacturer _mainManufacturer = ManufacturersData.MainManufacturer;
    
    [Fact]
    public async Task ShouldCreateManufacturer()
    {
        // Arrange
        
        var facultyName = "From Test Manufacturer";
        var request = new ManufacturerDto(
            Id: null,
            Name: facultyName);

        // Act
        
        var response = await Client.PostAsJsonAsync("manufacturers", request);

        // Assert
        
        response.IsSuccessStatusCode.Should().BeTrue();

        var manufacturerFromResponse = await response.ToResponseModel<ManufacturerDto>();
        var manufacturerId = new ManufacturerId(manufacturerFromResponse.Id!.Value);

        var manufacturerFromDataBase = await Context.Manufacturers.
            FirstOrDefaultAsync(x => x.Id == manufacturerId);
        
        manufacturerFromDataBase.Should().NotBeNull();

        manufacturerFromDataBase!.Name.Should().Be(facultyName);
    }

    [Fact]
    public async Task ShouldUpdateManufacturer()
    {
        // Arrange
        var newFacultyName = "New Manufacturer Name";
        var request = new ManufacturerDto(
            Id: _mainManufacturer.Id.Value,
            Name: newFacultyName);

        // Act
        var response = await Client.PutAsJsonAsync("manufacturers", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var manufacturerFromResponse = await response.ToResponseModel<ManufacturerDto>();

        var manufacturerFromDataBase = await Context.Manufacturers
            .FirstOrDefaultAsync(x => x.Id == new ManufacturerId(manufacturerFromResponse.Id!.Value));

        manufacturerFromDataBase.Should().NotBeNull();

        manufacturerFromDataBase!.Name.Should().Be(newFacultyName);
    }

    [Fact]
    public async Task ShouldNotCreateManufacturerBecauseNameDuplicated()
    {
        // Arrange
        var request = new ManufacturerDto(
            Id: null,
            Name: _mainManufacturer.Name);

        // Act
        var response = await Client.PostAsJsonAsync("manufacturers", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ShouldNotUpdateManufacturerBecauseManufacturerNotFound()
    {
        // Arrange
        var request = new ManufacturerDto(
            Id: Guid.NewGuid(),
            Name: "New Manufacturer Name");

        // Act
        var response = await Client.PutAsJsonAsync("manufacturers", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task ShouldDeleteManufacturer()
    {
        // Arrange
        var manufacturerId = _mainManufacturer.Id;

        var existingManufacturer = await Context.Manufacturers
            .FirstOrDefaultAsync(x => x.Id == manufacturerId);

        existingManufacturer.Should().NotBeNull();
        
        // Act
        var response = await Client.DeleteAsync($"manufacturers/{manufacturerId.Value}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var manufacturerFromDataBase = await Context.Manufacturers
            .FirstOrDefaultAsync(x => x.Id == manufacturerId);
        manufacturerFromDataBase.Should().BeNull();
    }
    
    public async Task InitializeAsync()
    {
        await Context.Manufacturers.AddAsync(_mainManufacturer);

        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Manufacturers.RemoveRange(Context.Manufacturers);

        await SaveChangesAsync();
    }
}