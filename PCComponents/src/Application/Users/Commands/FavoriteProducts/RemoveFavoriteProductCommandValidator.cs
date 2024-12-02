using FluentValidation;

namespace Application.Users.Commands.FavoriteProducts;

public class RemoveFavoriteProductCommandValidator: AbstractValidator<RemoveFavoriteProductCommand>
{
    public RemoveFavoriteProductCommandValidator()
    {
        RuleFor(p => p.UserId).NotEmpty();
        RuleFor(p => p.ProductId).NotEmpty();
    }
}