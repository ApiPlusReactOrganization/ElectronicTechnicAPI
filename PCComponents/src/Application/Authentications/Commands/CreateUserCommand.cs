using Application.Authentications.Exceptions;
using Application.Authentications.Services;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Authentications.Users;
using MediatR;
using Domain.Authentications;

namespace Application.Authentications.Commands;

public class CreateUserCommand : IRequest<Result<User, AuthenticationException>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string Name { get; init; }
}

public class CreateUserCommandHandler(
    IUserRepository userRepository)
    : IRequestHandler<CreateUserCommand, Result<User, AuthenticationException>>
{
    public async Task<Result<User, AuthenticationException>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.SearchByEmail(request.Email, cancellationToken);

        return await existingUser.Match(
            u => Task.FromResult<Result<User, AuthenticationException>>(new UserByThisEmailAlreadyExistsException(u.Id)),
            async () => await CreateEntity(request.Email, request.Password, request.Name, cancellationToken));
    }

    private async Task<Result<User, AuthenticationException>> CreateEntity(
        string email,
        string password,
        string name,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = User.New(UserId.New(), email, name, HashPasswordService.HashPassword(password));
            await userRepository.Create(entity, cancellationToken);
            return await userRepository.AddRole(entity.Id, AuthSettings.UserRole, cancellationToken);
        }
        catch (Exception exception)
        {
            return new AuthenticationUnknownException(UserId.Empty, exception);
        }
    }


    
}