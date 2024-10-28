using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Products.Exceptions;
using Domain.Categories;
using Domain.ComponentCharacteristics;
using Domain.Manufacturers;
using Domain.Products;
using MediatR;

namespace Application.Products.Commands;

public record CreateProductCommand : IRequest<Result<Product, ProductException>>
{
    public required string Name { get; init; }
    public decimal Price { get; init; }
    public string? Description { get; init; }
    public int StockQuantity { get; init; }
    public ManufacturerId ManufacturerId { get; init; }
    public CategoryId CategoryId { get; init; }
    
    public Case Case { get; init; }
}

public class CreateProductCommandHandler(
    IProductRepository ProductRepository)
    : IRequestHandler<CreateProductCommand, Result<Product, ProductException>>
{
    public async Task<Result<Product, ProductException>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var existingProduct = await ProductRepository.SearchByName(request.Name, cancellationToken);

        return await existingProduct.Match(
            p => Task.FromResult<Result<Product, ProductException>>(new ProductAlreadyExistsException(p.Id)),
            async () => await CreateEntity(request.Name, request.Price, request.Description, request.StockQuantity,
                request.ManufacturerId, request.CategoryId, request.Case, cancellationToken));
    }

    private async Task<Result<Product, ProductException>> CreateEntity(
        string name,
        decimal price,
        string description,
        int stockQuantity,
        ManufacturerId manufacturerId,
        CategoryId categoryId,
        Case Case,
        CancellationToken cancellationToken)
    {
        try
        {
            
            
            var entity = Product.New(ProductId.New(), name, price, description, stockQuantity, manufacturerId,
                categoryId, ComponentCharacteristic.NewCaseCharacteristic(Case));
            return await ProductRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ProductUnknownException(ProductId.Empty, exception);
        }
    }
}