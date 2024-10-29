﻿using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Manufacturers.Commands;
using Domain.Authentications;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("manufacturers")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = AuthSettings.AdminRole)]
[ApiController]
public class ManufacturersController(ISender sender, IManufacturerQueries ManufacturerQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ManufacturerDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await ManufacturerQueries.GetAll(cancellationToken);

        return entities.Select(ManufacturerDto.FromDomainModel).ToList();
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
            f => ManufacturerDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }
}
