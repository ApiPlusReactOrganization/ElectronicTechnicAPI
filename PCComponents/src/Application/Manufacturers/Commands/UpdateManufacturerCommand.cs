using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Manufacturers.Exceptions;
using Domain.Categories;
using Domain.Manufacturers;
using MediatR;
using Optional;

namespace Application.Manufacturers.Commands;

public record UpdateManufacturerCommand : IRequest<Result<Manufacturer, ManufacturerException>>
{
    public required Guid ManufacturerId { get; init; }
    public required string Name { get; init; }
    public required List<Guid> Categories { get; init; }
}

public class UpdateManufacturerCommandHandler(
    IManufacturerRepository manufacturerRepository,
    ICategoryQueries categoryQueries)
    : IRequestHandler<UpdateManufacturerCommand, Result<Manufacturer, ManufacturerException>>
{
    public async Task<Result<Manufacturer, ManufacturerException>> Handle(
        UpdateManufacturerCommand request,
        CancellationToken cancellationToken)
    {
        var manufacturerId = new ManufacturerId(request.ManufacturerId);
        var existingManufacturer = await manufacturerRepository.GetById(manufacturerId, cancellationToken);

        var categoryList = new List<Category>();
        foreach (var categoryId in request.Categories)
        {
            var existingCategory = await categoryQueries.GetById(new CategoryId(categoryId), cancellationToken);

            var categoryResult = await existingCategory.Match<Task<Result<Category, ManufacturerException>>>(
                async c =>
                {
                    categoryList.Add(c);
                    return c;
                },
                () => Task.FromResult<Result<Category, ManufacturerException>>(
                    new CategoryNotFoundException(new CategoryId(categoryId)))
            );

            if (categoryResult.IsError)
            {
                return new ManufacturerUnknownException(ManufacturerId.Empty, new Exception("Error with update manufacturer"));;
            }
        }

        return await existingManufacturer.Match<Task<Result<Manufacturer, ManufacturerException>>>(
            async manufacturer => await UpdateManufacturer(manufacturer, request.Name, categoryList, cancellationToken),
            () => Task.FromResult<Result<Manufacturer, ManufacturerException>>(
                new ManufacturerNotFoundException(manufacturerId))
        );
    }

    private async Task<Result<Manufacturer, ManufacturerException>> UpdateManufacturer(
        Manufacturer manufacturer,
        string name,
        List<Category> categories,
        CancellationToken cancellationToken)
    {
        try
        {
            manufacturer.UpdateName(name);
            manufacturer.SetCategories(categories);

            return await manufacturerRepository.Update(manufacturer, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ManufacturerUnknownException(manufacturer.Id, exception);
        }
    }
}
