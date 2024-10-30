using Api.Dtos.Authentications;
using Api.Modules.Errors;
using Application.Authentications;
using Application.Authentications.Commands;
using Application.Common.Interfaces.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountController(ISender sender, IUserQueries userQueries) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<ActionResult<SignUpDto>> SignUpAsync(
        [FromBody] SignUpDto request,
        CancellationToken cancellationToken)
    {
        var input = new SignUpCommand
        {
            Email = request.email,
            Password = request.password,
            Name = request.name,
        };
        
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<SignUpDto>>(
            f => SignUpDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }
    
    [HttpPost("signin")]
    public async Task<ActionResult<ServiceResponse>> SignUpAsync(
        [FromBody] SignInDto request,
        CancellationToken cancellationToken)
    {
        var input = new SignInCommand
        {
            Email = request.email,
            Password = request.password
        };
        
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ServiceResponse>>(
            f => f,
            e => e.ToObjectResult());
    }
}