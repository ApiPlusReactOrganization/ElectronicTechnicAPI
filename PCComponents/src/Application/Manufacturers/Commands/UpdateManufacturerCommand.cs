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
        var manufacturer = await manufacturerRepository.GetById(manufacturerId, cancellationToken);

        return await manufacturer.Match(
            async f =>
            {
                var existingManufacturer = await CheckDuplicated(
                    manufacturerId, request.Name, cancellationToken);

                return await existingManufacturer.Match(
                    m => Task.FromResult<Result<Manufacturer, ManufacturerException>>
                        (new ManufacturerAlreadyExistsException(m.Id)),
                    async () => await UpdateEntity(f, request.Name, cancellationToken));
            },
            () => Task.FromResult<Result<Manufacturer, ManufacturerException>>
                (new ManufacturerNotFoundException(manufacturerId)));
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

    private async Task<Option<Manufacturer>> CheckDuplicated(
        ManufacturerId facultyId,
        string name,
        CancellationToken cancellationToken)
    {
        var faculty = await manufacturerRepository.SearchByName(name, cancellationToken);

        return faculty.Match(
            m => m.Id == facultyId ? Option.None<Manufacturer>() : Option.Some(m),
            Option.None<Manufacturer>);
    }
}