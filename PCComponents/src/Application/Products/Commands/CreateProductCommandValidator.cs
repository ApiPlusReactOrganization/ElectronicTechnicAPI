using Application.Products.ComponentCharacteristics;
using Domain.Products;
using FluentValidation;
using FluentValidation.Validators;

namespace Application.Products.Commands;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(255)
            .MinimumLength(3)
            .WithMessage("Name must be between 3 and 255 characters.");

        RuleFor(x => x.ComponentCharacteristic)
            .NotEmpty()
            .WithMessage("Please specify a component characteristic.");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID cannot be empty");

        RuleFor(x => x.ManufacturerId)
            .NotEmpty()
            .WithMessage("ManufacturerId should not be empty");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .PrecisionScale(8, 2,false)
            .WithMessage("Price must be up to 999999.99 with optional decimal places.");

        RuleFor(x => x.Description)
            .MinimumLength(3)
            .MaximumLength(1000)
            .WithMessage("Description must be between 3 and 1000 characters.");

        RuleFor(x => x.StockQuantity)
            .GreaterThan(0)
            .LessThanOrEqualTo(1000)
            .WithMessage("Stock quantity must be between 0 and 1000.");
        
        When(x => x.ComponentCharacteristic?.Case != null, () =>
        {
            RuleFor(x => x.ComponentCharacteristic.Case)
                .SetValidator(new CreateCaseValidator());
        });
        
        When(x => x.ComponentCharacteristic?.Cpu != null, () =>
        {
            RuleFor(x => x.ComponentCharacteristic.Cpu)
                .SetValidator(new CreateCpuValidator());
        });
        
        When(x => x.ComponentCharacteristic?.Gpu != null, () =>
        {
            RuleFor(x => x.ComponentCharacteristic.Gpu)
                .SetValidator(new CreateGpuValidator());
        });

        When(x => x.ComponentCharacteristic?.Motherboard != null, () =>
        {
            RuleFor(x => x.ComponentCharacteristic.Motherboard)
                .SetValidator(new CreateMotherboardValidator());
        });

        When(x => x.ComponentCharacteristic is { Psu: { } }, () =>
        {
            RuleFor(x => x.ComponentCharacteristic.Psu)
                .SetValidator(new CreatePsuValidator());
        });
        
        When(x => x.ComponentCharacteristic is { Ram: { } }, () =>
        {
            RuleFor(x => x.ComponentCharacteristic.Ram)
                .SetValidator(new CreateRamValidator());
        });

        When(x => x.ComponentCharacteristic is { Cooler: { } }, () =>
        {
            RuleFor(x => x.ComponentCharacteristic.Cooler)
                .SetValidator(new CreateCoolerValidator());
        });

        When(x => x.ComponentCharacteristic is { Hdd: { } }, () =>
        {
            RuleFor(x => x.ComponentCharacteristic.Hdd)
                .SetValidator(new CreateHddValidator());
        });

        When(x => x.ComponentCharacteristic is { Ssd: { } }, () =>
        {
            RuleFor(x => x.ComponentCharacteristic.Ssd)
                .SetValidator(new CreateSsdValidator());
        });
    }
}