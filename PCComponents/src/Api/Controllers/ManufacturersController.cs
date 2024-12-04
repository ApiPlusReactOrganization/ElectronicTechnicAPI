using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Manufacturers.Commands;
using Domain.Authentications;
using Domain.Categories;
using Domain.Manufacturers;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("manufacturers")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = AuthSettings.AdminRole)]
[ApiController]
public class ManufacturersController(
    ISender sender,
    IManufacturerQueries manufacturerQueries,
    ICategoryQueries categoryQueries) 
    : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ActionResult<IReadOnlyList<ManufacturerDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await manufacturerQueries.GetAll(cancellationToken);

        return entities.Select(ManufacturerDto.FromDomainModel).ToList();
    }

    [HttpGet("get-by-id/{manufacturerId:guid}")]
    public async Task<ActionResult<ManufacturerDto>> Get([FromRoute] Guid manufacturerId,
        CancellationToken cancellationToken)
    {
        var entity = await manufacturerQueries.GetById(
            new ManufacturerId(manufacturerId), cancellationToken);

        return entity.Match<ActionResult<ManufacturerDto>>(
            m => ManufacturerDto.FromDomainModel(m),
            () => NotFound());
    }

    [HttpGet("get-by-category-id/{categoryId:guid}")]
    public async Task<ActionResult<IReadOnlyList<ManufacturerDto>>> GetByCategory([FromRoute] Guid categoryId,
        CancellationToken cancellationToken)
    {
        var category = await categoryQueries.GetById(
            new CategoryId(categoryId), cancellationToken);

        return category.Match<ActionResult<IReadOnlyList<ManufacturerDto>>>(
            m => m.Manufacturers.Select(ManufacturerDto.FromDomainModel).ToList(),
            () => NotFound());
    }

    [HttpPost("create")]
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

    [HttpPut("update")]
    public async Task<ActionResult<ManufacturerDto>> Update(
        [FromBody] ManufacturerDto request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateManufacturerCommand()
        {
            ManufacturerId = request.Id!.Value,
            Name = request.Name, 
            Categories = request.Categories.Select(c => c.Id!.Value).ToList(),
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ManufacturerDto>>(
            m => ManufacturerDto.FromDomainModel(m),
            e => e.ToObjectResult());
    }

    [HttpDelete("delete/{manufacturerId:guid}")]
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