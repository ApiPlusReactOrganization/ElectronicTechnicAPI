using FluentValidation;

namespace Application.CartItems.Commands;

public class DeleteCartItemCommandValidator: AbstractValidator<DeleteCartItemCommand>
{
    public DeleteCartItemCommandValidator()
    {
        RuleFor(x => x.CartItemId)
            .NotEmpty()
            .WithMessage("Cart item ID is required");
    }
}