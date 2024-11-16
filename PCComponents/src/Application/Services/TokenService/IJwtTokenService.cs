using Domain.Authentications.Users;

namespace Application.Authentications.Services.TokenService
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
