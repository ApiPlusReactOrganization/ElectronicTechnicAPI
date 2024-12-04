using Api.Dtos;
using Api.Modules.Errors;
using Application.Categories.Commands;
using Application.Common.Interfaces.Queries;
using Domain.Authentications;
using Domain.Categories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("categories")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = AuthSettings.AdminRole)]
[ApiController]
public class CategoriesController(ISender sender, ICategoryQueries categoryQueries) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await categoryQueries.GetAll(cancellationToken);

        return entities.Select(CategoryDto.FromDomainModel).ToList();
    }
    
    [AllowAnonymous]
    [HttpGet("getById/{categoryId:guid}")]
    public async Task<ActionResult<CategoryDto>> Get([FromRoute] Guid categoryId, CancellationToken cancellationToken)
    {
        var entity = await categoryQueries.GetById(
            new CategoryId(categoryId), cancellationToken);

        return entity.Match<ActionResult<CategoryDto>>(
            c => CategoryDto.FromDomainModel(c),
            () => NotFound());
    }
}