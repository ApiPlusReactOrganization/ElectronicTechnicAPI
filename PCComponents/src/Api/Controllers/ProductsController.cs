using Api.Dtos.Products;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Products.Commands;
using Domain.Authentications;
using Domain.Categories;
using Domain.Manufacturers;
using Domain.Products;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("products")]
/*[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = AuthSettings.AdminRole)]*/
[ApiController]
public class ProductsController(ISender sender, IProductQueries productQueries) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await productQueries.GetAll(cancellationToken);

        return entities.Select(ProductDto.FromDomainModel).ToList();
    }

    [HttpGet("under-category-and-manufacturer/{categoryId:guid}/{manufacturerId:guid}")]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetByCategoryAndManufacturer(
        [FromRoute] Guid categoryId,
        [FromRoute] Guid manufacturerId,
        CancellationToken cancellationToken)
    {
        var products = await productQueries.GetProductsByCategoryAndManufacturer(
            new CategoryId(categoryId),
            new ManufacturerId(manufacturerId),
            cancellationToken);

        return products.Select(ProductDto.FromDomainModel).ToList();
    }
    
    [HttpGet("under-category/{categoryId:guid}")]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetByCategory(
        [FromRoute] Guid categoryId,
        CancellationToken cancellationToken)
    {
        var products = await productQueries.GetProductsByCategory(
            new CategoryId(categoryId),
            cancellationToken);

        return products.Select(ProductDto.FromDomainModel).ToList();
    }
    
    [HttpGet("under-manufacturer/{manufacturerId:guid}")]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetByManufacturer(
        [FromRoute] Guid manufacturerId,
        CancellationToken cancellationToken)
    {
        var products = await productQueries.GetProductsByManufacturer(
            new ManufacturerId(manufacturerId),
            cancellationToken);

        return products.Select(ProductDto.FromDomainModel).ToList();
    }

    [HttpGet("{productId:guid}")]
    public async Task<ActionResult<ProductDto>> Get([FromRoute] Guid productId, CancellationToken cancellationToken)
    {
        var entity = await productQueries.GetById(
            new ProductId(productId), cancellationToken);

        return entity.Match<ActionResult<ProductDto>>(
            p => ProductDto.FromDomainModel(p),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<CreateProductDto>> Create(
        [FromBody] CreateProductDto request,
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
            ComponentCharacteristic = request.ComponentCharacteristic,
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CreateProductDto>>(
            f => CreateProductDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }

    [HttpDelete("{productId:guid}")]
    public async Task<ActionResult<ProductDto>>
        Delete([FromRoute] Guid productId, CancellationToken cancellationToken)
    {
        var input = new DeleteProductCommand()
        {
            ProductId = productId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ProductDto>>(
            p => Ok(ProductDto.FromDomainModel(p)),
            e => e.ToObjectResult());
    }
    
    [HttpPut("{productId:guid}")]
    public async Task<ActionResult<CreateProductDto>> Update(
        [FromRoute] Guid productId,
        [FromBody] CreateProductDto request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateProductCommand()
        {
            ProductId = productId,
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            StockQuantity = request.StockQuantity,
            ManufacturerId = request.ManufacturerId,
            CategoryId = request.CategoryId,
            ComponentCharacteristic = request.ComponentCharacteristic,
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CreateProductDto>>(
            f => CreateProductDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }
    
    [HttpPut("UploadImages/{productId:guid}")]
    public async Task<ActionResult<ProductDto>> Upload([FromRoute] Guid productId, IFormFileCollection imagesFiles,
        CancellationToken cancellationToken)
    {
        var input = new UploadProductImagesCommand()
        {
            ProductId = productId,
            ImagesFiles = imagesFiles
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<ProductDto>>(
            r => ProductDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }
}