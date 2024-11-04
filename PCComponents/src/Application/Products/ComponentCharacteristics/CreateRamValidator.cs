using Domain.Products;
using Domain.Products.PCComponents;
using FluentValidation;

namespace Application.Products.ComponentCharacteristics;

public class CreateRamValidator: AbstractValidator<RAM>
{
    public CreateRamValidator()
    {
        RuleFor(x => x.MemoryAmount)
            .GreaterThan(0)
            .WithMessage("Memory amount must be greater than zero.");

        RuleFor(x => x.MemorySpeed)
            .GreaterThan(0)
            .WithMessage("Memory speed must be greater than zero.");

        RuleFor(x => x.MemoryType)
            .MinimumLength(3)
            .MaximumLength(255)
            .WithMessage("Memory type must be between 3 and 255 characters.");

        RuleFor(x => x.FormFactor)
            .MinimumLength(3)
            .MaximumLength(255)
            .WithMessage("Form factor must be between 3 and 255 characters.");

        RuleFor(x => x.Voltage)
            .GreaterThan(0)
            .WithMessage("Voltage must be greater than zero.");

        RuleFor(x => x.MemoryBandwidth)
            .GreaterThan(0)
            .WithMessage("Memory bandwidth must be greater than zero.");
    }
}