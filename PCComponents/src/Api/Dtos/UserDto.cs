using Domain.Auth.Users;

namespace Api.Dtos;

public record UserDto(UserId id, string email, string? name, string? image, List<RoleDto>? roles)
{
    public static UserDto FromDomainModel(User user)
    => new(user.Id, user.Email, user.Name, user.Image, user.Roles.Select(RoleDto.FromDomainModel).ToList());
}