using FluentValidation;

namespace Application.Products.Commands;

public class UpdateStockQuantityForProductCommandValidator : AbstractValidator<UpdateStockQuantityForProductCommand>
{
    public UpdateStockQuantityForProductCommandValidator()
    {
        RuleFor(x=>x.ProductId).NotEmpty().WithMessage("Product ID cannot be empty");
        
        RuleFor(x=>x.StockQuantity).NotEmpty().WithMessage("Stock quantity cannot be empty");
    }
}