using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Authentications.Users;
using MediatR;

namespace Application.Users.Commands;

public class DeleteUserCommand : IRequest<Result<User, UserException>>
{
    public required Guid UserId { get; init; }
}

public class DeleteUserCommandHandler(
    IUserRepository UserRepository)
    : IRequestHandler<DeleteUserCommand, Result<User, UserException>>
{
    public async Task<Result<User, UserException>> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        var UserId = new UserId(request.UserId);
        var existingUser = await UserRepository.GetById(UserId, cancellationToken);

        return await existingUser.Match<Task<Result<User, UserException>>>(
            async User => await DeleteEntity(User, cancellationToken),
            () => Task.FromResult<Result<User, UserException>>
                (new UserNotFoundException(UserId)));
    }

    private async Task<Result<User, UserException>> DeleteEntity(
        User User,
        CancellationToken cancellationToken)
    {
        try
        {
            return await UserRepository.Delete(User, cancellationToken);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(User.Id, exception);
        }
    }
}