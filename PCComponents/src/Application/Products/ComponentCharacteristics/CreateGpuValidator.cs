using Domain.Products;
using Domain.Products.PCComponents;
using FluentValidation;

namespace Application.Products.ComponentCharacteristics;

public class CreateGpuValidator: AbstractValidator<GPU>
{
    public CreateGpuValidator()
    {
        RuleFor(x => x.Model)
            .MinimumLength(3)
            .MaximumLength(255)
            .WithMessage("Model must be between 3 and 255 characters.");

        RuleFor(x => x.MemorySize)
            .GreaterThan(0)
            .WithMessage("Memory size must be greater than zero.");

        RuleFor(x => x.MemoryType)
            .MinimumLength(3)
            .MaximumLength(1000)
            .WithMessage("Memory type description must be more than 0 and less than 1000 characters.");

        RuleFor(x => x.CoreClock)
            .GreaterThan(0)
            .WithMessage("Core clock must be greater than zero.");

        RuleFor(x => x.BoostClock)
            .GreaterThan(0)
            .WithMessage("Boost clock must be greater than zero.");

        RuleFor(x => x.FormFactor)
            .MinimumLength(3)
            .MaximumLength(255)
            .WithMessage("Form factor must be more than 0 and less than 255 characters.");
    }
}