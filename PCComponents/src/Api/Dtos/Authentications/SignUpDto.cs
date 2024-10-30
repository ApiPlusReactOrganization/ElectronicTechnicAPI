using Domain.Authentications.Users;

namespace Api.Dtos.Authentications;

public record SignUpDto(string email, string password, string? name)
{
    public static SignUpDto FromDomainModel(User user)
        => new(user.Email, user.PasswordHash, user.Name);
}