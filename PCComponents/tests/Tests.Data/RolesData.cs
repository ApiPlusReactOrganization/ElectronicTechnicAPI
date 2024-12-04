using Api.Dtos.Users;
using Domain.Authentications.Roles;

namespace Tests.Data;

public static class RolesData
{
    public static RoleDto AdminRole => new( "Administrator");
}