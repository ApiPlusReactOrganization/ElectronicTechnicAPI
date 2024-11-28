using Api.Dtos.Users;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Services;
using Application.Users.Commands;
using Domain.Authentications;
using Domain.Authentications.Users;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("users")]
// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
// [Authorize(Roles = AuthSettings.AdminRole)]
[ApiController]
public class UsersController(ISender sender, IUserQueries userQueries) : ControllerBase
{
    [HttpGet("get-all")]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await userQueries.GetAll(cancellationToken);

        return entities.Select(UserDto.FromDomainModel).ToList();
    }
    
    [HttpGet("get-by-id/{userId:guid}")]
    public async Task<ActionResult<UserDto>> Get([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var entity = await userQueries.GetById(new UserId(userId), cancellationToken);

        return entity.Match<ActionResult<UserDto>>(
            p => UserDto.FromDomainModel(p),
            () => NotFound());
    }

    [HttpDelete("delete/{userId:guid}")]
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

    [HttpPut("update-roles/{userId}")]
    public async Task<ActionResult<UserDto>>
        UpdateRoles([FromRoute] Guid userId, [FromBody] List<RoleDto> roles, CancellationToken cancellationToken)
    {
        var input = new ChangeRolesForUserCommand()
        {
            UserId = userId,
            Roles = roles.Select(x => x.Name.ToString()).ToList()
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<UserDto>>(
            c => UserDto.FromDomainModel(c),
            e => e.ToObjectResult());
    }

    [HttpPut("image/{userId}")]
    public async Task<ActionResult<ServiceResponseForJwtToken>> Upload([FromRoute] Guid userId, IFormFile imageFile,
        CancellationToken cancellationToken)
    {
        var input = new UploadUserImageCommand
        {
            UserId = userId,
            ImageFile = imageFile
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ServiceResponseForJwtToken>>(
            r => r,
            e => e.ToObjectResult());
    }

    [HttpPut("update/{userId:guid}")]
    public async Task<ActionResult<ServiceResponseForJwtToken>> UpdateUser([FromRoute] Guid userId,
        [FromBody] UpdateUserVM user,
        CancellationToken cancellationToken)
    {
        var input = new UpdateUserCommand()
        {
            UserId = userId,
            UserName = user.UserName,
            Email = user.Email
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ServiceResponseForJwtToken>>(
            r => r,
            e => e.ToObjectResult());
    }
}