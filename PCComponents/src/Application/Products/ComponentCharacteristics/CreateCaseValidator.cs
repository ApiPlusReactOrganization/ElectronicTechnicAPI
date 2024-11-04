using Domain.Products;
using Domain.Products.PCComponents;
using FluentValidation;

namespace Application.Products.ComponentCharacteristics;

public class CreateCaseValidator: AbstractValidator<Case>
{
    public CreateCaseValidator()
    {
        RuleFor(x => x.NumberOfFans)
            .GreaterThan(0)
            .WithMessage("Number of fans must be greater than zero.");

        RuleFor(x => x.CoolingDescription)
            .MinimumLength(3)
            .MaximumLength(1000)
            .WithMessage("Cooling system must be more than 0 and less than 1000 characters.");

        RuleFor(x => x.FormFactor)
            .MinimumLength(3)
            .MaximumLength(1000)
            .WithMessage("Form factor must be more than 0 and less than 1000 characters.");

        RuleFor(x => x.PortsDescription)
            .MinimumLength(3)
            .MaximumLength(1000)
            .WithMessage("Ports description must be more than 0 and less than 1000 characters.");

        RuleFor(x => x.CompartmentDescription)
            .MinimumLength(3)
            .MaximumLength(1000)
            .WithMessage("Compartment description must be more than 0 and less than 1000 characters.");
    }
}