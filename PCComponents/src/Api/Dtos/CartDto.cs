using Domain.Categories;

namespace Api.Dtos;

public record CartDto(Guid? Id, string Name)
{
    public static CategoryDto FromDomainModel(Category category)
        => new(category.Id.Value, category.Name);
}