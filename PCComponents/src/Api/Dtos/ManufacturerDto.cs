using Domain.Manufacturers;

namespace Api.Dtos;

public record ManufacturerDto(Guid? Id, string Name, List<CategoryDto> Categories)
{
    public static ManufacturerDto FromDomainModel(Manufacturer manufacturer)
        => new(manufacturer.Id.Value, manufacturer.Name,
            manufacturer.Categories.Select(CategoryDto.FromDomainModel).ToList());
}