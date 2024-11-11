using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Categories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Categories
{
    public class CategoriesControllerTests
        : BaseIntegrationTest, IAsyncLifetime
    {
        private IntegrationTestWebFactory factory {get; set;}

        public CategoriesControllerTests(IntegrationTestWebFactory factory) : base(factory)
        {
            this.factory = factory;
        
            var token = GenerateJwtToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        
        private readonly Category _mainCategory = CategoriesData.MainCategory;

        [Fact]
        public async Task ShouldCreateCategory()
        {
            // Arrange
            var categoryName = "From Test Category";
            var request = new CategoryDto(
                Id: null,
                Name: categoryName);

            // Act
            var response = await Client.PostAsJsonAsync("categories", request);

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var categoryFromResponse = await response.ToResponseModel<CategoryDto>();
            var categoryId = new CategoryId(categoryFromResponse.Id!.Value);

            var categoryFromDataBase = await Context.Categories
                .FirstOrDefaultAsync(x => x.Id == categoryId);

            categoryFromDataBase.Should().NotBeNull();
            categoryFromDataBase!.Name.Should().Be(categoryName);
        }

        [Fact]
        public async Task ShouldUpdateCategory()
        {
            // Arrange
            var newCategoryName = "New Category Name";
            var request = new CategoryDto(
                Id: _mainCategory.Id.Value,
                Name: newCategoryName);

            // Act
            var response = await Client.PutAsJsonAsync("categories", request);

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var categoryFromResponse = await response.ToResponseModel<CategoryDto>();

            var categoryFromDataBase = await Context.Categories
                .FirstOrDefaultAsync(x => x.Id == new CategoryId(categoryFromResponse.Id!.Value));

            categoryFromDataBase.Should().NotBeNull();
            categoryFromDataBase!.Name.Should().Be(newCategoryName);
        }

        [Fact]
        public async Task ShouldNotCreateCategoryBecauseNameDuplicated()
        {
            // Arrange
            var request = new CategoryDto(
                Id: null,
                Name: _mainCategory.Name);

            // Act
            var response = await Client.PostAsJsonAsync("categories", request);

            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task ShouldNotUpdateCategoryBecauseCategoryNotFound()
        {
            // Arrange
            var request = new CategoryDto(
                Id: Guid.NewGuid(),
                Name: "New Category Name");

            // Act
            var response = await Client.PutAsJsonAsync("categories", request);

            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldDeleteCategory()
        {
            // Arrange
            var categoryId = _mainCategory.Id;

            var existingCategory = await Context.Categories
                .FirstOrDefaultAsync(x => x.Id == categoryId);

            existingCategory.Should().NotBeNull();

            // Act
            var response = await Client.DeleteAsync($"categories/{categoryId.Value}");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var categoryFromDataBase = await Context.Categories
                .FirstOrDefaultAsync(x => x.Id == categoryId);
            categoryFromDataBase.Should().BeNull();
        }

        public async Task InitializeAsync()
        {
            await Context.Categories.AddAsync(_mainCategory);
            await SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            Context.Categories.RemoveRange(Context.Categories);
            await SaveChangesAsync();
        }
    }
}