using Api.Dtos.Users;
using Domain.Authentications.Users;

namespace Tests.Data;

public static class UsersData
{
    
    public static UserDto TestUser => new(
        Guid.NewGuid(),
         "testuser@example.com",
       "Test User",
        null,
        new List<RoleDto> { new ("User") });
}