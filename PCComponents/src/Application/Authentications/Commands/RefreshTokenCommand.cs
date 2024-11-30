using Application.Authentications.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Services.TokenService;
using Application.ViewModels;
using Domain.Authentications.Users;
using Domain.RefreshTokens;
using MediatR;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Application.Authentications.Commands;

public record RefreshTokenCommand : IRequest<Result<JwtVM, AuthenticationException>>
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}

public class RefreshTokenCommandHandler(
    IJwtTokenService jwtTokenService,
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository
)
    : IRequestHandler<RefreshTokenCommand, Result<JwtVM, AuthenticationException>>
{
    public async Task<Result<JwtVM, AuthenticationException>> Handle(RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var existingRefreshToken =
            await refreshTokenRepository.GetRefreshTokenAsync(request.RefreshToken, cancellationToken);

        return await existingRefreshToken.Match(
            async rt => await RefreshToken(rt, request.AccessToken, cancellationToken),
            () => Task.FromResult<Result<JwtVM, AuthenticationException>>(
                new InvalidTokenException()));
    }

    private async Task<Result<JwtVM, AuthenticationException>> RefreshToken(RefreshToken storedToken,
        string accessToken, CancellationToken cancellationToken)
    {
        if (storedToken.IsUsed)
        {
            return await Task.FromResult<Result<JwtVM, AuthenticationException>>(new InvalidTokenException());
        }

        if (storedToken.ExpiredDate < DateTime.UtcNow)
        {
            return await Task.FromResult<Result<JwtVM, AuthenticationException>>(new TokenExpiredException());
        }

        var principals = jwtTokenService.GetPrincipals(accessToken);

        var accessTokenId = principals.Claims
            .Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

        if (storedToken.JwtId != accessTokenId)
        {
            return await Task.FromResult<Result<JwtVM, AuthenticationException>>(new InvalidAccessTokenException());
        }

        var existingUser = await userRepository.GetById(storedToken.UserId, cancellationToken);

        return await existingUser.Match<Task<Result<JwtVM, AuthenticationException>>>(
            async u => await jwtTokenService.GenerateTokensAsync(u, cancellationToken),
            () => Task.FromResult<Result<JwtVM, AuthenticationException>>(
                new UserNorFoundException(storedToken.UserId)));
    }
}