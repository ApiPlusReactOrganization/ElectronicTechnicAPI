using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Services;
using Application.Users.Exceptions;
using Domain.Authentications.Users;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Application.Users.Commands;

public record DeleteUserCommand : IRequest<Result<User, UserException>>
{
    public required Guid UserId { get; init; }
}

public class DeleteUserCommandHandler(
    IUserRepository userRepository, IWebHostEnvironment webHostEnvironment)
    : IRequestHandler<DeleteUserCommand, Result<User, UserException>>
{
    public async Task<Result<User, UserException>> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        if (userId == null) throw new ArgumentNullException(nameof(userId));
        var existingUser = await userRepository.GetById(userId, cancellationToken);

        return await existingUser.Match<Task<Result<User, UserException>>>(
            async user =>
            {
                if (user == null) throw new ArgumentNullException(nameof(user));
                return await DeleteEntity(user, cancellationToken);
            },
            () => Task.FromResult<Result<User, UserException>>
                (new UserNotFoundException(userId)));
    }

    private async Task<Result<User, UserException>> DeleteEntity(
        User user,
        CancellationToken cancellationToken)
    {
        DeleteImageByUser(user);
        
        try
        {
            return await userRepository.Delete(user, cancellationToken);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(user.Id, exception);
        }
    }

    private void DeleteImageByUser(User user)
    {
        var userImage = user.UserImage?.FilePath;
        if (!string.IsNullOrEmpty(userImage))
        {
            var fullPath = Path.Combine(webHostEnvironment.ContentRootPath, ImagePaths.UserImagePath, userImage);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}