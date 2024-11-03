using FluentValidation;
using FluentValidation.Validators;

namespace Application.Products.Commands;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).MaximumLength(255)
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
            .ScalePrecision(2, 8)
            .WithMessage("Price must be greater than zero and less than 999 999,99");
        
        RuleFor(x => x.Description)
            .MinimumLength(3)
            .MaximumLength(1000)
            .WithMessage("Description must be between 3 and 1000 characters");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(1000)
            .WithMessage("Stock quantity must be between 0 and 1000");
    }
}