using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Manufacturers.Exceptions;
using Domain.Manufacturers;
using MediatR;
using Optional;

namespace Application.Manufacturers.Commands;

public record UpdateManufacturerCommand : IRequest<Result<Manufacturer, ManufacturerException>>
{
    public required Guid ManufacturerId { get; init; }
    public required string Name { get; init; }
}

public class UpdateFacultyCommandHandler(
    IManufacturerRepository manufacturerRepository) :
    IRequestHandler<UpdateManufacturerCommand, Result<Manufacturer, ManufacturerException>>
{
    public async Task<Result<Manufacturer, ManufacturerException>> Handle(
        UpdateManufacturerCommand request,
        CancellationToken cancellationToken)
    {
        var manufacturerId = new ManufacturerId(request.ManufacturerId);
        var existingManufacturer = await manufacturerRepository.GetById(manufacturerId, cancellationToken);

        return await existingManufacturer.Match(
            async m => await UpdateEntity(m, request.Name, cancellationToken),
            () => Task.FromResult<Result<Manufacturer, ManufacturerException>>(
                new ManufacturerNotFoundException(manufacturerId)));
    }

    private async Task<Result<Manufacturer, ManufacturerException>> UpdateEntity(
        Manufacturer manufacturer,
        string name,
        CancellationToken cancellationToken)
    {
        try
        {
            manufacturer.UpdateName(name);

            return await manufacturerRepository.Update(manufacturer, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ManufacturerUnknownException(manufacturer.Id, exception);
        }
    }
}