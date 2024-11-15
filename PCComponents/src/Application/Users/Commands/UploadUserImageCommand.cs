using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Authentications.Users;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Optional;

namespace Application.Users.Commands;

public class UploadUserImageCommand : IRequest<Result<User, UserException>>
{
    public Guid UserId { get; init; }
    public Stream FileStream { get; init; }
}

public class UploadUserImageCommandHandler(IUserRepository userRepository, IWebHostEnvironment webHostEnvironment)
    : IRequestHandler<UploadUserImageCommand, Result<User, UserException>>
{
    public async Task<Result<User, UserException>> Handle(UploadUserImageCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var existingUser = await userRepository.GetById(userId, cancellationToken);

        return await existingUser.Match<Task<Result<User, UserException>>>(
            async user => await UploadOrReplaceImage(user, request.FileStream, cancellationToken),
            () => Task.FromResult<Result<User, UserException>>(new UserNotFoundException(userId)));
    }

    private async Task<Result<User, UserException>> UploadOrReplaceImage(
        User user,
        Stream fileStream,
        CancellationToken cancellationToken)
    {
        var oldImagePath = user.UserImage?.FilePath;

        var imageSaveResult = await SaveImageAsync(ImagePaths.UserImagePath, fileStream, oldImagePath);

        return await imageSaveResult.Match<Task<Result<User, UserException>>>(
            async imageName =>
            {
                var imageEntity = UserImage.New(UserImageId.New(), user.Id, imageName);
                user.UpdateUserImage(imageEntity);
                return await userRepository.Update(user, cancellationToken);
            },
            () => Task.FromResult<Result<User, UserException>>(new ImageSaveException(user.Id)));
    }

    private async Task<Option<string>> SaveImageAsync(string path, Stream imageStream, string? oldImagePath)
    {
        try
        {
            if (!string.IsNullOrEmpty(oldImagePath))
            {
                var fullOldPath = Path.Combine(webHostEnvironment.ContentRootPath, path, oldImagePath);
                if (File.Exists(fullOldPath))
                {
                    File.Delete(fullOldPath);
                }
            }

            var imageName = $"{Guid.NewGuid()}.jpeg";
            var filePath = Path.Combine(webHostEnvironment.ContentRootPath, path, imageName);

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await imageStream.CopyToAsync(fileStream);
            }

            return Option.Some(imageName);
        }
        catch (Exception ex)
        {
            return Option.None<string>();
        }
    }
}