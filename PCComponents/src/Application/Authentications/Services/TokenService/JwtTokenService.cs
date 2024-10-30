using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Authentications.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Authentications.Services.TokenService
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var issuer = _configuration["AuthSettings:issuer"];
            var audience = _configuration["AuthSettings:audience"];
            var keyString = _configuration["AuthSettings:key"];
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString!));

            var claims = new Claim[]
            {
                new Claim("id", user.Id.Value.ToString()),
                new Claim("email", user.Email!),
                new Claim("name", user.Name ?? "N/A"),
                new Claim("role", user.Roles.Count()! > 0 ? string.Join(',', user.Roles.Select(x=>x.Name)) : "N/A")
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
