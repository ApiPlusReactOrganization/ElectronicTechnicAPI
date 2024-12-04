using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Manufacturers.Exceptions;
using Domain.Manufacturers;
using MediatR;

namespace Application.Manufacturers.Commands;

public record CreateManufacturerCommand : IRequest<Result<Manufacturer, ManufacturerException>>
{
    public required string Name { get; init; }
}

public class CreateManufacturerCommandHandler(
    IManufacturerRepository manufacturerRepository)
    : IRequestHandler<CreateManufacturerCommand, Result<Manufacturer, ManufacturerException>>
{
    public async Task<Result<Manufacturer, ManufacturerException>> Handle(
        CreateManufacturerCommand request,
        CancellationToken cancellationToken)
    {
        var existingManufacturer = await manufacturerRepository.SearchByName(request.Name, cancellationToken);

        return await existingManufacturer.Match(
            c => Task.FromResult<Result<Manufacturer, ManufacturerException>>(
                new ManufacturerAlreadyExistsException(c.Id)),
            async () => await CreateEntity(request.Name, cancellationToken));
    }

    private async Task<Result<Manufacturer, ManufacturerException>> CreateEntity(
        string name,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = Manufacturer.New(ManufacturerId.New(), name);

            return await manufacturerRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ManufacturerUnknownException(ManufacturerId.Empty, exception);
        }
    }
}