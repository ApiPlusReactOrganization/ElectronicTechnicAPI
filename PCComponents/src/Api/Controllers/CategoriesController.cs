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
/*[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = AuthSettings.AdminRole)]*/
[ApiController]
public class CategoriesController(ISender sender, ICategoryQueries categoryQueries) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await categoryQueries.GetAll(cancellationToken);

        return entities.Select(CategoryDto.FromDomainModel).ToList();
    }
    
    [AllowAnonymous]
    [HttpGet("{categoryId:guid}")]
    public async Task<ActionResult<CategoryDto>> Get([FromRoute] Guid categoryId, CancellationToken cancellationToken)
    {
        var entity = await categoryQueries.GetById(
            new CategoryId(categoryId), cancellationToken);

        return entity.Match<ActionResult<CategoryDto>>(
            c => CategoryDto.FromDomainModel(c),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(
        [FromBody] CategoryDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateCategoryCommand
        {
            Name = request.Name,
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CategoryDto>>(
            f => CategoryDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }
    
    [HttpPut]
    [AllowAnonymous]
    public async Task<ActionResult<CategoryDto>> Update(
        [FromBody] CategoryDto request,
        [FromRoute] Guid categoryId,
        CancellationToken cancellationToken)
    {
        var input = new UpdateCategoryCommand()
        {
            CategoryId = categoryId,
            Name = request.Name
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CategoryDto>>(
            c => CategoryDto.FromDomainModel(c),
            e => e.ToObjectResult());
    }
    
    [HttpDelete("{categoryId:guid}")]
    public async Task<ActionResult<CategoryDto>> 
        Delete([FromRoute] Guid categoryId, CancellationToken cancellationToken)
    {
        var input = new DeleteCategoryCommand()
        {
            CategoryId = categoryId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CategoryDto>>(
            c => CategoryDto.FromDomainModel(c),
            e => e.ToObjectResult());
    }
}