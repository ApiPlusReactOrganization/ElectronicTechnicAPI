﻿using Application.Authentications.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Services;
using Application.Services.HashPasswordService;
using Application.Services.TokenService;
using Domain.Authentications.Users;
using MediatR;

namespace Application.Authentications.Commands;

public class SignInCommand: IRequest<Result<ServiceResponseForJwtToken, AuthenticationException>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class SignInCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService, IHashPasswordService hashPasswordService) 
    : IRequestHandler<SignInCommand, Result<ServiceResponseForJwtToken, AuthenticationException>>
{
    public async Task<Result<ServiceResponseForJwtToken, AuthenticationException>> Handle(
        SignInCommand request,
        CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.SearchByEmail(request.Email, cancellationToken);
        
        return await existingUser.Match(
            u => Task.FromResult(SignIn(u, request.Password, cancellationToken)),
            () => Task.FromResult<Result<ServiceResponseForJwtToken, AuthenticationException>>(new EmailOrPasswordAreIncorrect()));
    }
    private Result<ServiceResponseForJwtToken, AuthenticationException> SignIn(
         User user,
         string password,
         CancellationToken cancellationToken)
     {
         string storedHash = user.PasswordHash;

         if (!hashPasswordService.VerifyPassword(password, storedHash))
         {
             return new EmailOrPasswordAreIncorrect();
         }

         try
         {
             string token = jwtTokenService.GenerateToken(user);
             return ServiceResponseForJwtToken.GetResponse("You're logged in", token);
         }
         catch (Exception exception)
         {
             return new AuthenticationUnknownException(user.Id, exception);
         }
     }
}