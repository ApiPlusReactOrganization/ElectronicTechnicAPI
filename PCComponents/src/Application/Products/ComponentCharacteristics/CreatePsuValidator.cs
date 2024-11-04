using Domain.Products;
using Domain.Products.PCComponents;
using FluentValidation;

namespace Application.Products.ComponentCharacteristics;

public class CreatePsuValidator: AbstractValidator<PSU>
{
    public CreatePsuValidator()
    {
        RuleFor(x => x.PowerCapacity)
            .MinimumLength(3)
            .MaximumLength(255)
            .WithMessage("Power capacity must be between 3 and 255 characters.");

        RuleFor(x => x.InputVoltageRange)
            .MinimumLength(3)
            .MaximumLength(255)
            .WithMessage("Input voltage range must be between 3 and 255 characters.");

        RuleFor(x => x.FanTypeAndSize)
            .MinimumLength(3)
            .MaximumLength(255)
            .WithMessage("Fan type and size must be between 3 and 255 characters.");

        RuleFor(x => x.Protections)
            .MinimumLength(3)
            .MaximumLength(255)
            .WithMessage("Protections must be between 3 and 255 characters.");

        RuleFor(x => x.Connectors)
            .MinimumLength(3)
            .MaximumLength(255)
            .WithMessage("Connectors must be between 3 and 255 characters.");
    }
}