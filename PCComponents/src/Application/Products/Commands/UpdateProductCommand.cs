using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Products.Exceptions;
using Domain.Categories;
using Domain.Manufacturers;
using Domain.Products;
using Domain.Products.PCComponents;
using MediatR;

namespace Application.Products.Commands;

public record UpdateProductCommand : IRequest<Result<Product, ProductException>>
{
    public Guid ProductId { get; init; }
    public required string Name { get; init; }
    public decimal Price { get; init; }
    public string Description { get; init; }
    public int StockQuantity { get; init; }
    public required Guid ManufacturerId { get; init; }
    public required Guid CategoryId { get; init; }
    public ComponentCharacteristic ComponentCharacteristic { get; init; }
}

public class UpdateProductCommandHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IManufacturerRepository manufacturerRepository)
    : IRequestHandler<UpdateProductCommand, Result<Product, ProductException>>
{
    public async Task<Result<Product, ProductException>> Handle(UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var productId = new ProductId(request.ProductId);
        var manufacturerId = new ManufacturerId(request.ManufacturerId);
        var categoryId = new CategoryId(request.CategoryId);

        var existingCategory = await categoryRepository.GetById(categoryId, cancellationToken);
        var existingManufacturer = await manufacturerRepository.GetById(manufacturerId, cancellationToken);
        var existingProduct = await productRepository.GetById(productId, cancellationToken);

        return await existingManufacturer.Match<Task<Result<Product, ProductException>>>(
            async manufacturer =>
            {
                return await existingCategory.Match(
                    async category =>
                    {
                        return await existingProduct.Match(
                            async product => await UpdateProduct(product, request, category, manufacturerId, cancellationToken),
                            () => Task.FromResult<Result<Product, ProductException>>(
                                new ProductNotFoundException(productId)));
                    },
                    () => Task.FromResult<Result<Product, ProductException>>(
                        new ProductCategoryNotFoundException(categoryId)));
            },
            () => Task.FromResult<Result<Product, ProductException>>(
                new ProductManufacturerNotFoundException(manufacturerId))
        );
    }

    private static ComponentCharacteristic ValidateAndCreateCharacteristic(
        Category category,
        ComponentCharacteristic componentCharacteristic)
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
            throw new ProductInvalidCategoryException(category.Id, category.Name);
        }

        return category.Name switch
        {
            PCComponentsNames.Case => ComponentCharacteristic.NewCase(componentCharacteristic.Case!),
            PCComponentsNames.CPU => ComponentCharacteristic.NewCpu(componentCharacteristic.Cpu!),
            PCComponentsNames.GPU => ComponentCharacteristic.NewGpu(componentCharacteristic.Gpu!),
            PCComponentsNames.Motherboard => ComponentCharacteristic.NewMotherboard(
                componentCharacteristic.Motherboard!),
            PCComponentsNames.RAM => ComponentCharacteristic.NewRam(componentCharacteristic.Ram!),
            PCComponentsNames.PSU => ComponentCharacteristic.NewPsu(componentCharacteristic.Psu!),
            PCComponentsNames.Cooler => ComponentCharacteristic.NewCooler(componentCharacteristic.Cooler!),
            PCComponentsNames.HDD => ComponentCharacteristic.NewHdd(componentCharacteristic.Hdd!),
            PCComponentsNames.SSD => ComponentCharacteristic.NewSSD(componentCharacteristic.Ssd!),
            _ => throw new ArgumentException("Invalid component type")
        };
    }

    private async Task<Result<Product, ProductException>> UpdateProduct(Product product, UpdateProductCommand request,
        Category category, ManufacturerId manufacturerId, CancellationToken cancellationToken)
    {
        try
        {
            product.UpdateDetails(
                request.Name,
                request.Price,
                request.Description,
                request.StockQuantity,
                category.Id,
                manufacturerId,
                ValidateAndCreateCharacteristic(category, request.ComponentCharacteristic)
            );

            var result = await productRepository.SaveChanges(cancellationToken);

            if (result > 0)
            {
                return product;
            }

            return new ProductUnknownException(product.Id, new Exception("Product was not updated"));
        }
        catch (Exception exception)
        {
            return new ProductUnknownException(ProductId.Empty, exception);
        }
    }
}