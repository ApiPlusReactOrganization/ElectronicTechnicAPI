using FluentAssertions;
using Tests.Common;
using Xunit;

namespace Api.Tests.Integration.Files;

public class FilesControllerTests(IntegrationTestWebFactory factory)
    : BaseIntegrationTest(factory), IAsyncLifetime
{
    [Fact]
    public async Task ShouldUploadSingleFile()
    {
        // Arrange
        var request = new MultipartFormDataContent();
        request.Add(new StreamContent(new MemoryStream()), "imageFile", "random image.jped");

        // Act
        var response = await Client.PostAsync("files", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldUploadMultipleFiles()
    {
        // Arrange
        var request = new MultipartFormDataContent();
        request.Add(new StreamContent(new MemoryStream()), "files", "random image1.jped");
        request.Add(new StreamContent(new MemoryStream()), "files", "random image2.jped");

        // Act
        var response = await Client.PostAsync("files/multiple", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => Task.CompletedTask;
}