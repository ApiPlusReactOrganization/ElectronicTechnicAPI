using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class ManufacturerDtoValidator : AbstractValidator<ManufacturerDto>
{
    public ManufacturerDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255).MinimumLength(3);
    }
}