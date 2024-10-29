using Domain.Authentications.Roles;

namespace Api.Dtos.Authentication;

public record RoleDto(string name)
{
    public static RoleDto FromDomainModel(Role role)
        => new(role.Name);
}