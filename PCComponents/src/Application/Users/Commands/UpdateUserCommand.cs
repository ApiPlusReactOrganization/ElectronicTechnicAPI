using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Services;
using Application.Services.TokenService;
using Application.Users.Exceptions;
using Domain.Authentications.Users;
using MediatR;

namespace Application.Users.Commands;

public record UpdateUserCommand : IRequest<Result<ServiceResponseForJwtToken, UserException>>
{
    public required Guid UserId { get; init; }
    public required string Email { get; init; }
    public required string UserName { get; init; }
}

public class UpdateUserCommandHandle(IUserRepository userRepository, IJwtTokenService jwtTokenService) : IRequestHandler<UpdateUserCommand, Result<ServiceResponseForJwtToken, UserException>>
{
    public async Task<Result<ServiceResponseForJwtToken, UserException>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var existingUser = await userRepository.GetById(userId, cancellationToken);

        return await existingUser.Match(
            async u =>
            {
                var existingEmail = await userRepository.SearchByEmailForUpdate(userId, request.Email, cancellationToken);

                return await existingEmail.Match(
                     e => Task.FromResult<Result<ServiceResponseForJwtToken, UserException>>
                         (new UserByThisEmailAlreadyExistsException(userId)),
                     async () => await UpdateEntity(u, request.Email, request.UserName, cancellationToken));
            },
            () => Task.FromResult<Result<ServiceResponseForJwtToken, UserException>>
                (new UserNotFoundException(userId)));
    }
    private async Task<Result<ServiceResponseForJwtToken, UserException>> UpdateEntity(
        User user,
        string email,
        string userName,
        CancellationToken cancellationToken)
    {
        try
        {
            user.UpdateUser(email, userName);

            var updatedUser = await userRepository.Update(user, cancellationToken);
            return ServiceResponseForJwtToken.GetResponse("User updated successfully",
                jwtTokenService.GenerateToken(updatedUser));
        }
        catch (Exception exception)
        {
            return new UserUnknownException(user.Id, exception);
        }
    }
}