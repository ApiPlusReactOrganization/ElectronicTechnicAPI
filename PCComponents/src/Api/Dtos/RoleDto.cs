using Domain.Auth.Roles;

namespace Api.Dtos;

public record RoleDto(string name)
{
    public static RoleDto FromDomainModel(Role role)
        => new(role.Name);
}