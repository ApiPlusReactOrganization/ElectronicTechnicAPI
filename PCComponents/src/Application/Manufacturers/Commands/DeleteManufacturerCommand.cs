using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Manufacturers.Exceptions;
using Domain.Manufacturers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Application.Manufacturers.Commands;

public record DeleteManufacturerCommand : IRequest<Result<Manufacturer, ManufacturerException>>
{
    public required Guid ManufacturerId { get; init; }
}

public class DeleteManufacturerCommandHandler(
    IManufacturerRepository manufacturerRepository)
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
            return await manufacturerRepository.Delete(manufacturer, cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23503")
        {
            return new ManufacturerHasRelatedProductsException(manufacturer.Id);
        }
        catch (Exception exception)
        {
            return new ManufacturerUnknownException(manufacturer.Id, exception);
        }
    }
}