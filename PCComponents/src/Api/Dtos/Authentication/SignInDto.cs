using Domain.Authentications.Users;

namespace Api.Dtos.Authentication;

public record SignInDto(string email, string password)
{
    public static SignInDto FromDomainModel(User user)
        => new(user.Email, user.PasswordHash);
}