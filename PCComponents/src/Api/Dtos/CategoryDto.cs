using Domain.Categories;
using Domain.Manufacturers;

namespace Api.Dtos;

public record CategoryDto(Guid? Id, string Name)
{
    public static CategoryDto FromDomainModel(Category category)
        => new(category.Id.Value, category.Name);
}