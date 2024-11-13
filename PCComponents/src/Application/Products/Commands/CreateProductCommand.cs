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
    public required Guid ManufacturerId { get; init; }
    public required Guid CategoryId { get; init; }
    public ComponentCharacteristic ComponentCharacteristic { get; init; }
}

public class CreateProductCommandHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IManufacturerRepository manufacturerRepository)
    : IRequestHandler<CreateProductCommand, Result<Product, ProductException>>
{
    public async Task<Result<Product, ProductException>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var manufacturerId = new ManufacturerId(request.ManufacturerId);
        var categoryId = new CategoryId(request.CategoryId);

        var existingCategory = await categoryRepository.GetById(categoryId, cancellationToken);
        var existingManufacturer = await manufacturerRepository.GetById(manufacturerId, cancellationToken);


        return await existingManufacturer.Match<Task<Result<Product, ProductException>>>(
            async m =>
            {
                return await existingCategory.Match(
                    async c =>
                    {
                        var existingProduct =
                            await productRepository.SearchByName(request.Name, c.Id, cancellationToken);
                        
                        return await existingProduct.Match(
                            p => Task.FromResult<Result<Product, ProductException>>
                                (new ProductUnderCurrentCategoryAlreadyExistsException(categoryId)),
                            
                            async () => await CreateEntity(request.Name, request.Price, request.Description,
                                request.StockQuantity,
                                manufacturerId, c, request.ComponentCharacteristic, cancellationToken));
                    },
                    () => Task.FromResult<Result<Product, ProductException>>(
                        new ProductCategoryNotFoundException(categoryId)));
            },
            () => Task.FromResult<Result<Product, ProductException>>(
                new ProductManufacturerNotFoundException(manufacturerId))
        );
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
            var isValidCategory = category.Name switch
            {
                PCComponentsNames.Case => componentCharacteristic.Case != null,
                PCComponentsNames.CPU => componentCharacteristic.Cpu != null,
                PCComponentsNames.GPU => componentCharacteristic.Gpu != null,
                PCComponentsNames.Motherboard => componentCharacteristic.Motherboard != null,
                PCComponentsNames.RAM => componentCharacteristic.Ram != null,
                PCComponentsNames.PSU => componentCharacteristic.Psu != null,
                PCComponentsNames.Cooler => componentCharacteristic.Cooler != null,
                PCComponentsNames.HDD => componentCharacteristic.Hdd != null,
                PCComponentsNames.SSD => componentCharacteristic.Ssd != null,
                _ => false
            };

            if (!isValidCategory)
            {
                return new ProductInvalidCategoryException(category.Id, category.Name);
            }

            ComponentCharacteristic characteristic = category.Name switch
            {
                PCComponentsNames.Case => ComponentCharacteristic.NewCase(componentCharacteristic.Case!),
                PCComponentsNames.CPU => ComponentCharacteristic.NewCpu(componentCharacteristic.Cpu!),
                PCComponentsNames.GPU => ComponentCharacteristic.NewGpu(componentCharacteristic.Gpu!),
                PCComponentsNames.Motherboard => ComponentCharacteristic.NewMotherboard(componentCharacteristic
                    .Motherboard!),
                PCComponentsNames.RAM => ComponentCharacteristic.NewRam(componentCharacteristic.Ram!),
                PCComponentsNames.PSU => ComponentCharacteristic.NewPsu(componentCharacteristic.Psu!),
                PCComponentsNames.Cooler => ComponentCharacteristic.NewCooler(componentCharacteristic.Cooler!),
                PCComponentsNames.HDD => ComponentCharacteristic.NewHdd(componentCharacteristic.Hdd!),
                PCComponentsNames.SSD => ComponentCharacteristic.NewSSD(componentCharacteristic.Ssd!),
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

            return await productRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ProductUnknownException(ProductId.Empty, exception);
        }
    }
}