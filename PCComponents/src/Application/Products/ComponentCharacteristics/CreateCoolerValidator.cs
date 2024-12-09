using Domain.Products;
using Domain.Products.PCComponents;
using FluentValidation;

namespace Application.Products.ComponentCharacteristics;

public class CreateCoolerValidator: AbstractValidator<Cooler>
{
    public CreateCoolerValidator()
    {
        RuleFor(x => x.Material)
            .MinimumLength(3)
            .MaximumLength(255)
            .WithMessage("Material must be between 3 and 255 characters.");

        RuleFor(x => x.Fanspeed)
            .GreaterThan(0)
            .WithMessage("Fan speed must be greater than zero.");

        RuleFor(x => x.FanAmount)
            .GreaterThan(0)
            .WithMessage("Fan amount must be greater than zero.");

        RuleFor(x => x.Voltage)
            .GreaterThan(0)
            .WithMessage("Voltage must be greater than zero.");

        RuleFor(x => x.MaxTDP)
            .GreaterThan(0)
            .WithMessage("Max TDP must be greater than zero.");

        // RuleFor(x => x.Sockets)
        //     .NotEmpty()
        //     .WithMessage("Sockets list cannot be empty.")
        //     .Must(sockets => sockets.All(socket => !string.IsNullOrWhiteSpace(socket)))
        //     .WithMessage("All socket values must be non-empty.");

        RuleFor(x => x.FanSupply)
            .MinimumLength(3)
            .MaximumLength(255)
            .WithMessage("Fan supply must be between 3 and 255 characters.");
    }
}