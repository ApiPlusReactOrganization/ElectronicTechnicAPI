using Application.Products.Commands;
using Domain.Products;
using Domain.Products.PCComponents;
using FluentValidation;

namespace Application.Products.ComponentCharacteristics;

public class CreateCpuValidator: AbstractValidator<CPU>
{
    public CreateCpuValidator()
    {
        RuleFor(x => x.Model)
            .MinimumLength(3)
            .MaximumLength(255)
            .WithMessage("Model must be between 3 and 255 characters.");

        RuleFor(x => x.Cores)
            .GreaterThan(0)
            .WithMessage("Cores must be greater than zero.");

        RuleFor(x => x.Threads)
            .GreaterThan(0)
            .WithMessage("Threads must be greater than zero.");

        RuleFor(x => x.BaseClock)
            .GreaterThan(0)
            .WithMessage("Base clock must be greater than zero.");

        RuleFor(x => x.BoostClock)
            .GreaterThan(0)
            .WithMessage("Boost clock must be greater than zero.");

        RuleFor(x => x.Socket)
            .MinimumLength(3)
            .MaximumLength(255)
            .WithMessage("Socket must be between 3 and 255 characters.");
    }
}