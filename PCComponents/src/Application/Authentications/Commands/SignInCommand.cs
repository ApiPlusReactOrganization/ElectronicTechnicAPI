using Application.Authentications.Exceptions;
using Application.Authentications.Services;
using Application.Authentications.Services.TokenService;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Authentications.Users;
using MediatR;

namespace Application.Authentications.Commands;

public class SignInCommand: IRequest<Result<ServiceResponse, AuthenticationException>>
{
    public string Email { get; init; }
    public string Password { get; init; }
}

public class SignInCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService) 
    : IRequestHandler<SignInCommand, Result<ServiceResponse, AuthenticationException>>
{
    public async Task<Result<ServiceResponse, AuthenticationException>> Handle(
        SignInCommand request,
        CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.SearchByEmail(request.Email, cancellationToken);

        return await existingUser.Match(
            async u => await SignIn(u, request.Password, jwtTokenService, cancellationToken),
            () => Task.FromResult<Result<ServiceResponse, AuthenticationException>>(new EmailOrPasswordAreIncorrect()));
    }
    private async Task<Result<ServiceResponse, AuthenticationException>> SignIn(
         User user,
         string password,
         IJwtTokenService jwtTokenService,
         CancellationToken cancellationToken)
     {
         string storedHash = user.PasswordHash;

         if (!HashPasswordService.VerifyPassword(password, storedHash))
         {
             return new EmailOrPasswordAreIncorrect();
         }

         try
         {
             string token = jwtTokenService.GenerateToken(user);
             return ServiceResponse.GetResponse("You're logged in", token);
         }
         catch (Exception exception)
         {
             return new AuthenticationUnknownException(user.Id, exception);
         }
     }
}