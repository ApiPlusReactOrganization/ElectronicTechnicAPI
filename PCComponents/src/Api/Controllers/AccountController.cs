using Api.Dtos.Authentications;
using Api.Modules.Errors;
using Application.Authentications;
using Application.Authentications.Commands;
using Application.Common.Interfaces.Queries;
using Application.Services;
using Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("account")]
[ApiController]
public class AccountController(ISender sender) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<ActionResult<JwtVM>> SignUpAsync(
        [FromBody] SignUpDto request,
        CancellationToken cancellationToken)
    {
        var input = new SignUpCommand
        {
            Email = request.Email,
            Password = request.Password,
            Name = request.Name,
        };
        
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<JwtVM>>(
            f => f,
            e => e.ToObjectResult());
    }
    
    [HttpPost("signin")]
    public async Task<ActionResult<JwtVM>> SignUpAsync(
        [FromBody] SignInDto request,
        CancellationToken cancellationToken)
    {
        var input = new SignInCommand
        {
            Email = request.Email,
            Password = request.Password
        };
        
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<JwtVM>>(
            f => f,
            e => e.ToObjectResult());
    }
    
    [HttpPost("refresh")]
    public async Task<ActionResult<JwtVM>> RefreshTokensAsync([FromBody] JwtVM model, CancellationToken cancellationToken)
    {
        var input = new RefreshTokenCommand()
        {
            AccessToken = model.AccessToken,
            RefreshToken = model.RefreshToken
        };
        
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<JwtVM>>(
            f => f,
            e => e.ToObjectResult());
    }
}