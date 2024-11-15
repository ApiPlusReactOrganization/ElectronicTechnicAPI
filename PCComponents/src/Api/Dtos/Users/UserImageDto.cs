using System.Net.Mime;
using Domain.Authentications.Users;

namespace Api.Dtos.Users;

public record UserImageDto(Guid? id, string filePath)
{
    public static UserImageDto FromDomainModel(UserImage userImage)
    => new(userImage.Id.Value, userImage.FilePath);
}