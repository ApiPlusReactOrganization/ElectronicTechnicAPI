using Domain.Authentications.Users;

namespace Api.Dtos.Users;

public record UserDto(Guid? id, string email, string? name, UserImageDto? image, List<RoleDto>? roles)
{
    public static UserDto FromDomainModel(User user)
    => new(user.Id.Value, user.Email, user.Name, user.UserImage != null ? UserImageDto.FromDomainModel(user.UserImage) : null, user.Roles.Select(RoleDto.FromDomainModel).ToList());
}