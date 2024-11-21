using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Authentications.Roles;
using Domain.Authentications.Users;
using MediatR;

namespace Application.Users.Commands;

public record ChangeRolesForUserCommand : IRequest<Result<User, UserException>>
{
    public required Guid UserId { get; init; }
    public List<string> Roles { get; init; }
}

public class ChangeRolesForUserCommandHandler(
    IUserRepository userRepository,
    IRoleQueries roleQueries)
    : IRequestHandler<ChangeRolesForUserCommand, Result<User, UserException>>
{
    public async Task<Result<User, UserException>> Handle(
        ChangeRolesForUserCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var existingUser = await userRepository.GetById(userId, cancellationToken);

        var rolesList = new List<Role>();
        foreach (var role in request.Roles)
        {
            var existingRole = await roleQueries.GetByName(role, cancellationToken);

            var roleResult = await existingRole.Match<Task<Result<Role, UserException>>>(
                async r =>
                {
                    rolesList.Add(r);
                    return r;
                },
                () => Task.FromResult<Result<Role, UserException>>(new RoleNotFoundException(role))
            );

            if (roleResult.IsError)
            {
                return new RoleNotFoundException(role);
            }
        }


        return await existingUser.Match<Task<Result<User, UserException>>>(
            async user => await ChangeRolesForUser(user, rolesList, cancellationToken),
            () => Task.FromResult<Result<User, UserException>>
                (new UserNotFoundException(userId)));
    }

    private async Task<Result<User, UserException>> ChangeRolesForUser(
        User user,
        List<Role> roles,
        CancellationToken cancellationToken)
    {
       
        try
        {
            user.Roles.Clear();
            user.Roles.AddRange(roles);
            
            return await userRepository.Update(user, cancellationToken);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(user.Id, exception);
        }
    }
}