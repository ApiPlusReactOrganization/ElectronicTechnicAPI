using Domain.Products;
using Domain.Products.PCComponents;
using FluentValidation;

namespace Application.Products.ComponentCharacteristics;

public class CreateSsdValidator: AbstractValidator<SSD>
{
    public CreateSsdValidator()
    {
        RuleFor(x => x.MemoryAmount)
            .GreaterThan(0)
            .WithMessage("Memory amount must be greater than zero.");

        RuleFor(x => x.FormFactor)
            .MinimumLength(3)
            .MaximumLength(255)
            .WithMessage("Form factor must be between 3 and 255 characters.");

        RuleFor(x => x.ReadSpeed)
            .GreaterThan(0)
            .WithMessage("Read speed must be greater than zero.");

        RuleFor(x => x.WriteSpeed)
            .GreaterThan(0)
            .WithMessage("Write speed must be greater than zero.");

        RuleFor(x => x.MaxTBW)
            .GreaterThan(0)
            .WithMessage("Max TBW (Terabytes Written) must be greater than zero.");
    }
}