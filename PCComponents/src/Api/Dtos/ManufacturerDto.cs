using Domain.Manufacturers;

namespace Api.Dtos;


public record ManufacturerDto(Guid? Id, string Name)
{
    public static ManufacturerDto FromDomainModel(Manufacturer manufacturer)
        => new(manufacturer.Id.Value, manufacturer.Name);
}