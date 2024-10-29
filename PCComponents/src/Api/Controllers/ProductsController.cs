using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Products.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("products")]
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
        var input = new CreateProductCommand
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            StockQuantity = request.StockQuantity,
            ManufacturerId = request.ManufacturerId,
            CategoryId = request.CategoryId,
            componentCharacteristic = request.ComponentCharacteristic,
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ProductDto>>(
            f => ProductDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }
}