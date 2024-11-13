using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Manufacturers.Exceptions;
using Domain.Manufacturers;
using MediatR;

namespace Application.Manufacturers.Commands;

public record DeleteManufacturerCommand : IRequest<Result<Manufacturer, ManufacturerException>>
{
    public required Guid ManufacturerId { get; init; }
}

public class DeleteManufacturerCommandHandler(
    IManufacturerRepository manufacturerRepository,
    IProductRepository productRepository)
    : IRequestHandler<DeleteManufacturerCommand, Result<Manufacturer, ManufacturerException>>
{
    public async Task<Result<Manufacturer, ManufacturerException>> Handle(
        DeleteManufacturerCommand request,
        CancellationToken cancellationToken)
    {
        var manufacturerId = new ManufacturerId(request.ManufacturerId);
        var existingManufacturer = await manufacturerRepository.GetById(manufacturerId, cancellationToken);

        return await existingManufacturer.Match<Task<Result<Manufacturer, ManufacturerException>>>(
            async manufacturer => await DeleteEntity(manufacturer, cancellationToken),
            () => Task.FromResult<Result<Manufacturer, ManufacturerException>>
                (new ManufacturerNotFoundException(manufacturerId)));
    }

    private async Task<Result<Manufacturer, ManufacturerException>> DeleteEntity(
        Manufacturer manufacturer,
        CancellationToken cancellationToken)
    {
        try
        {
            var hasRelatedProducts = await productRepository
                .HasProductsInManufacturerAsync(manufacturer.Id, cancellationToken);

            if (hasRelatedProducts)
            {
                return new ManufacturerHasRelatedProductsException(manufacturer.Id);
            }
            
            return await manufacturerRepository.Delete(manufacturer, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ManufacturerUnknownException(manufacturer.Id, exception);
        }
    }
}