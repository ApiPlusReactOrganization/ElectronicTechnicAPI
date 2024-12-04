using System.Net;
using System.Net.Http.Json;
using Api.Dtos.Authentications;
using Api.Dtos.Users;
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
}