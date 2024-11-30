using System.Net;
using System.Net.Http.Json;
using Api.Dtos.Authentications;
using Application.Authentications;
using Application.Services;
using Application.ViewModels;
using FluentAssertions;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Accounts;

public class AccountControllerTests(IntegrationTestWebFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task ShouldSignUp()
    {
        // Arrange
        var request = AccountData.SignUpRequest;

        // Act
        var response = await Client.PostAsJsonAsync("account/signup", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = await response.Content.ReadFromJsonAsync<JwtVM>();
        content.Should().NotBeNull();
        content!.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldNotSignUpWithInvalidEmail()
    {
        // Arrange
        var request = AccountData.SignUpWithInvalidEmailRequest;

        // Act
        var response = await Client.PostAsJsonAsync("account/signup", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldNotSignUpWithoutPassword()
    {
        // Arrange
        var request = AccountData.SignUpWithOutPasswordRequest;

        // Act
        var response = await Client.PostAsJsonAsync("account/signup", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task ShouldNotSignUpWithoutName()
    {
        // Arrange
        var request = AccountData.SignUpWithoutNameRequest;

        // Act
        var response = await Client.PostAsJsonAsync("account/signup", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task ShouldSignIn()
    {
        // Arrange
        var signUpRequest = AccountData.SignUpForSignInRequest;
        await Client.PostAsJsonAsync("account/signup", signUpRequest);

        var signInRequest = AccountData.SignInRequest;

        // Act
        var response = await Client.PostAsJsonAsync("account/signin", signInRequest);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = await response.Content.ReadFromJsonAsync<JwtVM>();
        content.Should().NotBeNull();
        content!.Should().NotBeNull();
    }


    [Fact]
    public async Task ShouldNotSignInWithInvalidCredentials()
    {
        // Arrange
        var signUpRequest = AccountData.SignUpForSignInRequest;
        await Client.PostAsJsonAsync("account/signup", signUpRequest);
        
        var request = AccountData.SignInWithInvalidCredentialsRequest;

        // Act
        var response = await Client.PostAsJsonAsync("account/signin", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ShouldNotSignInWithInvalidEmail()
    {
        // Arrange
        var signUpRequest = AccountData.SignUpForSignInRequest;
        await Client.PostAsJsonAsync("account/signup", signUpRequest);
        
        var request = AccountData.SignInWithInvalidEmailRequest;

        // Act
        var response = await Client.PostAsJsonAsync("account/signin", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ShouldNotSignInWithoutPassword()
    {
        // Arrange
        var signUpRequest = AccountData.SignUpForSignInRequest;
        await Client.PostAsJsonAsync("account/signup", signUpRequest);
        
        var request = AccountData.SignInWithoutPasswordRequest;

        // Act
        var response = await Client.PostAsJsonAsync("account/signin", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
