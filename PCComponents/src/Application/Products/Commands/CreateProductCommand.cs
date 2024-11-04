using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Products.Exceptions;
using Domain;
using Domain.Categories;
using Domain.Manufacturers;
using Domain.Products;
using Domain.Products.PCComponents;
using MediatR;

namespace Application.Products.Commands;

public record CreateProductCommand : IRequest<Result<Product, ProductException>>
{
    public required string Name { get; init; }
    public decimal Price { get; init; }
    public string? Description { get; init; }
    public int StockQuantity { get; init; }
    
    //todo, id переробити в просто guid, як в красюка в прикладі
    public ManufacturerId ManufacturerId { get; init; }
    public CategoryId CategoryId { get; init; }
    public ComponentCharacteristic ComponentCharacteristic { get; init; }
}

public class CreateProductCommandHandler(
    IProductRepository ProductRepository,
    ICategoryRepository CategoryRepository)
    : IRequestHandler<CreateProductCommand, Result<Product, ProductException>>
{
    public async Task<Result<Product, ProductException>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var existingCategory = await CategoryRepository.GetById(request.CategoryId, cancellationToken);
        return await existingCategory.Match<Task<Result<Product, ProductException>>>(
            async c =>
            {
                var existingProduct = await ProductRepository.SearchByName(request.Name, cancellationToken);
                return await existingProduct.Match(
                p => Task.FromResult<Result<Product, ProductException>>
                    (new ProductUnderCurrentCategoryAlreadyExistsException(request.CategoryId)),
                async () => await CreateEntity(request.Name, request.Price, request.Description, request.StockQuantity,
                    request.ManufacturerId, c, request.ComponentCharacteristic, cancellationToken));
            },
            () => Task.FromResult<Result<Product, ProductException>>(new ProductCategoryNotFoundException(request.CategoryId)));

    }

    private async Task<Result<Product, ProductException>> CreateEntity(
        string name,
        decimal price,
        string description,
        int stockQuantity,
        ManufacturerId manufacturerId,
        Category category,
        ComponentCharacteristic componentCharacteristic,
        CancellationToken cancellationToken)
    {
        try
        {
            ComponentCharacteristic characteristic = category.Name switch
            {
                PCComponentsNames.Case => ComponentCharacteristic.NewCase(componentCharacteristic.Case),
                PCComponentsNames.CPU => ComponentCharacteristic.NewCpu(componentCharacteristic.Cpu),
                PCComponentsNames.GPU => ComponentCharacteristic.NewGpu(componentCharacteristic.Gpu),
                PCComponentsNames.Motherboard => ComponentCharacteristic.NewMotherboard(componentCharacteristic.Motherboard),
                PCComponentsNames.RAM => ComponentCharacteristic.NewRam(componentCharacteristic.Ram),
                PCComponentsNames.PSU => ComponentCharacteristic.NewPsu(componentCharacteristic.Psu),
                PCComponentsNames.Cooler => ComponentCharacteristic.NewCooler(componentCharacteristic.Cooler),
                PCComponentsNames.HDD => ComponentCharacteristic.NewHdd(componentCharacteristic.Hdd),
                PCComponentsNames.SSD => ComponentCharacteristic.NewSSD(componentCharacteristic.Ssd),
                _ => throw new ArgumentException("Invalid component type")
            };

            var entity = Product.New(
                ProductId.New(),
                name,
                price,
                description,
                stockQuantity,
                manufacturerId,
                category.Id,
                characteristic
            );

            return await ProductRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ProductUnknownException(ProductId.Empty, exception);
        }

    }
}