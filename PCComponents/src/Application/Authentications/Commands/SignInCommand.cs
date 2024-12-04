using Application.Authentications.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Services.HashPasswordService;
using Application.Services.TokenService;
using Application.ViewModels;
using Domain.Authentications.Users;
using MediatR;

namespace Application.Authentications.Commands;

public class SignInCommand: IRequest<Result<JwtVM, AuthenticationException>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class SignInCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService, IHashPasswordService hashPasswordService) 
    : IRequestHandler<SignInCommand, Result<JwtVM, AuthenticationException>>
{
    public async Task<Result<JwtVM, AuthenticationException>> Handle(
        SignInCommand request,
        CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.SearchByEmail(request.Email, cancellationToken);
        
        return await existingUser.Match(
            async u => await SignIn(u, request.Password, cancellationToken),
            () => Task.FromResult<Result<JwtVM, AuthenticationException>>(new EmailOrPasswordAreIncorrectException()));
    }
    private async Task<Result<JwtVM, AuthenticationException>> SignIn(
         User user,
         string password,
         CancellationToken cancellationToken)
     {
         string storedHash = user.PasswordHash;

         if (!hashPasswordService.VerifyPassword(password, storedHash))
         {
             return new EmailOrPasswordAreIncorrectException();
         }

         try
         {
             var token = await jwtTokenService.GenerateTokensAsync(user, cancellationToken);
             return token;
         }
         catch (Exception exception)
         {
             return new AuthenticationUnknownException(user.Id, exception);
         }
     }
}