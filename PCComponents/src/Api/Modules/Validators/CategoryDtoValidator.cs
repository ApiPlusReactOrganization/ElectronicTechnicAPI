using Api.Dtos;
using Domain.Categories;
using FluentValidation;

namespace Api.Modules.Validators;

public class CategoryDtoValidator : AbstractValidator<CategoryDto>
{
    public CategoryDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255).MinimumLength(3);
    }
}