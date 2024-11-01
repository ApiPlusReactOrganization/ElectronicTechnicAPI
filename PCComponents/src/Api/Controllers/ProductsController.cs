using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Products.Commands;
using Domain.Authentications;
using Domain.Categories;
using Domain.Manufacturers;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("products")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = AuthSettings.AdminRole)]
[ApiController]
public class ProductsController(ISender sender, IProductQueries ProductQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await ProductQueries.GetAll(cancellationToken);

        return entities.Select(ProductDto.FromDomainModel).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(
        [FromBody] ProductDto request,
        CancellationToken cancellationToken)
    {
        //todo, id переробити в просто guid, як в красюка в прикладі
        var input = new CreateProductCommand
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            StockQuantity = request.StockQuantity,
            ManufacturerId = new ManufacturerId(request.ManufacturerId.Value),
            CategoryId = new CategoryId(request.CategoryId.Value),
            componentCharacteristic = request.ComponentCharacteristic,
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ProductDto>>(
            f => ProductDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }
}