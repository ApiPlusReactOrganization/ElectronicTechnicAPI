using Domain.Authentications.Users;

namespace Api.Dtos.Authentications;

public record SignUpDto(string Email, string Password, string? Name)
{
    public static SignUpDto FromDomainModel(User user)
        => new(user.Email, user.PasswordHash, user.Name);
}