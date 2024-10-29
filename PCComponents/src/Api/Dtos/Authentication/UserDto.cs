using Domain.Authentications.Users;

namespace Api.Dtos.Authentication;

public record UserDto(Guid? id, string email, string? name, string? image, List<RoleDto>? roles)
{
    public static UserDto FromDomainModel(User user)
    => new(user.Id.Value, user.Email, user.Name, user.Image, user.Roles.Select(RoleDto.FromDomainModel).ToList());
}