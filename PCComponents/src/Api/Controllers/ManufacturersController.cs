using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Manufacturers.Commands;
using Domain.Authentications;
using Domain.Manufacturers;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("manufacturers")]
/*[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = AuthSettings.AdminRole)]*/
[ApiController]
public class ManufacturersController(ISender sender, IManufacturerQueries manufacturerQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ManufacturerDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await manufacturerQueries.GetAll(cancellationToken);

        return entities.Select(ManufacturerDto.FromDomainModel).ToList();
    }
    
    [HttpGet("{manufacturerId:guid}")]
    public async Task<ActionResult<ManufacturerDto>> Get([FromRoute] Guid manufacturerId, CancellationToken cancellationToken)
    {
        var entity = await manufacturerQueries.GetById(
            new ManufacturerId(manufacturerId), cancellationToken);

        return entity.Match<ActionResult<ManufacturerDto>>(
            m => ManufacturerDto.FromDomainModel(m),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<ManufacturerDto>> Create(
        [FromBody] ManufacturerDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateManufacturerCommand
        {
            Name = request.Name,
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ManufacturerDto>>(
            m => ManufacturerDto.FromDomainModel(m),
            e => e.ToObjectResult());
    }
    
    [HttpPut]
    public async Task<ActionResult<ManufacturerDto>> Update(
        [FromBody] ManufacturerDto request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateManufacturerCommand()
        {
            ManufacturerId = request.Id!.Value,
            Name = request.Name
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ManufacturerDto>>(
            m => ManufacturerDto.FromDomainModel(m),
            e => e.ToObjectResult());
    }
    
    [HttpDelete("{manufacturerId:guid}")]
    public async Task<ActionResult<ManufacturerDto>> 
        Delete([FromRoute] Guid manufacturerId, CancellationToken cancellationToken)
    {
        var input = new DeleteManufacturerCommand()
        {
            ManufacturerId = manufacturerId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ManufacturerDto>>(
            m => ManufacturerDto.FromDomainModel(m),
            e => e.ToObjectResult());
    }
}
