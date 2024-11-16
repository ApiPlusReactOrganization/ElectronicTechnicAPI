using Api.Dtos.Authentications;
using Api.Modules.Errors;
using Application.Authentications;
using Application.Authentications.Commands;
using Application.Common.Interfaces.Queries;
using Application.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountController(ISender sender) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<ActionResult<ServiceResponseForJwtToken>> SignUpAsync(
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

        return result.Match<ActionResult<ServiceResponseForJwtToken>>(
            f => f,
            e => e.ToObjectResult());
    }
    
    [HttpPost("signin")]
    public async Task<ActionResult<ServiceResponseForJwtToken>> SignUpAsync(
        [FromBody] SignInDto request,
        CancellationToken cancellationToken)
    {
        var input = new SignInCommand
        {
            Email = request.email,
            Password = request.password
        };
        
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ServiceResponseForJwtToken>>(
            f => f,
            e => e.ToObjectResult());
    }
}