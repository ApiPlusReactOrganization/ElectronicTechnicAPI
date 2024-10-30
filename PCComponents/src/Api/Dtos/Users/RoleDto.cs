using Domain.Authentications.Roles;

namespace Api.Dtos.Users;

public record RoleDto(string name)
{
    public static RoleDto FromDomainModel(Role role)
        => new(role.Name);
}