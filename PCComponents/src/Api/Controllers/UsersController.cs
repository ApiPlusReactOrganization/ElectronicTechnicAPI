using Api.Dtos.Users;
using Api.Modules.Errors;
using Application.Authentications;
using Application.Common.Interfaces.Queries;
using Application.Users.Commands;
using Domain.Authentications;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("[controller]")]
// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
// [Authorize(Roles = AuthSettings.AdminRole)]
[ApiController]
public class UsersController(ISender sender, IUserQueries userQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await userQueries.GetAll(cancellationToken);

        return entities.Select(UserDto.FromDomainModel).ToList();
    }

    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult<UserDto>>
        Delete([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var input = new DeleteUserCommand()
        {
            UserId = userId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<UserDto>>(
            c => UserDto.FromDomainModel(c),
            e => e.ToObjectResult());
    }

    [HttpPut("UpdateRoles/{userId}")]
    public async Task<ActionResult<UserDto>>
        UpdateRoles([FromRoute] Guid userId, [FromBody] List<RoleDto> roles, CancellationToken cancellationToken)
    {
        var input = new ChangeRolesForUserCommand()
        {
            UserId = userId,
            Roles = roles.Select(x => x.name.ToString()).ToList()
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<UserDto>>(
            c => UserDto.FromDomainModel(c),
            e => e.ToObjectResult());
    }

    [HttpPut("Image/{userId}")]
    public async Task<ActionResult<ServiceResponseForJwtToken>> Upload([FromRoute] Guid userId, IFormFile imageFile,
        CancellationToken cancellationToken)
    {
        var input = new UploadUserImageCommand
        {
            UserId = userId,
            FileStream = imageFile.OpenReadStream()
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ServiceResponseForJwtToken>>(
            r => r,
            e => e.ToObjectResult());
    }
}