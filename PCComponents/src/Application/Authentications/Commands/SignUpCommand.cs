using Application.Authentications.Exceptions;
using Application.Authentications.Services;
using Application.Authentications.Services.HashPasswordService;
using Application.Authentications.Services.TokenService;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Authentications.Users;
using MediatR;
using Domain.Authentications;

namespace Application.Authentications.Commands;

public class SignUpCommand : IRequest<Result<ServiceResponseForJwtToken, AuthenticationException>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string? Name { get; init; }
}

public class CreateUserCommandHandler(
    IUserRepository userRepository,
    IJwtTokenService jwtTokenService,
    IHashPasswordService hashPasswordService)
    : IRequestHandler<SignUpCommand, Result<ServiceResponseForJwtToken, AuthenticationException>>
{
    public async Task<Result<ServiceResponseForJwtToken, AuthenticationException>> Handle(
        SignUpCommand request,
        CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.SearchByEmail(request.Email, cancellationToken);

        return await existingUser.Match(
            u => Task.FromResult<Result<ServiceResponseForJwtToken, AuthenticationException>>(
                new UserByThisEmailAlreadyExistsException(u.Id)),
            async () => await SignUp(request.Email, request.Password, request.Name, cancellationToken));
    }

    private async Task<Result<ServiceResponseForJwtToken, AuthenticationException>> SignUp(
        string email,
        string password,
        string? name,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = User.New(UserId.New(), email, name, hashPasswordService.HashPassword(password));
            await userRepository.Create(entity, cancellationToken);

            string token =
                jwtTokenService.GenerateToken(await userRepository.AddRole(entity.Id, AuthSettings.UserRole,
                    cancellationToken));
            return ServiceResponseForJwtToken.GetResponse("You're sign up!", token);
        }
        catch (Exception exception)
        {
            return new AuthenticationUnknownException(UserId.Empty, exception);
        }
    }
}