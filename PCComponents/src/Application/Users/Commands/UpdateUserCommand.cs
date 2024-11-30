using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Services;
using Application.Services.TokenService;
using Application.Users.Exceptions;
using Application.ViewModels;
using Domain.Authentications.Users;
using MediatR;

namespace Application.Users.Commands;

public record UpdateUserCommand : IRequest<Result<JwtVM, UserException>>
{
    public required Guid UserId { get; init; }
    public required string Email { get; init; }
    public required string UserName { get; init; }
}

public class UpdateUserCommandHandle(IUserRepository userRepository, IJwtTokenService jwtTokenService) : IRequestHandler<UpdateUserCommand, Result<JwtVM, UserException>>
{
    public async Task<Result<JwtVM, UserException>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var existingUser = await userRepository.GetById(userId, cancellationToken);

        return await existingUser.Match(
            async u =>
            {
                var existingEmail = await userRepository.SearchByEmailForUpdate(userId, request.Email, cancellationToken);

                return await existingEmail.Match(
                     e => Task.FromResult<Result<JwtVM, UserException>>
                         (new UserByThisEmailAlreadyExistsException(userId)),
                     async () => await UpdateEntity(u, request.Email, request.UserName, cancellationToken));
            },
            () => Task.FromResult<Result<JwtVM, UserException>>
                (new UserNotFoundException(userId)));
    }
    private async Task<Result<JwtVM, UserException>> UpdateEntity(
        User user,
        string email,
        string userName,
        CancellationToken cancellationToken)
    {
        try
        {
            user.UpdateUser(email, userName);

            var updatedUser = await userRepository.Update(user, cancellationToken);
            return await jwtTokenService.GenerateTokensAsync(user, cancellationToken);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(user.Id, exception);
        }
    }
}