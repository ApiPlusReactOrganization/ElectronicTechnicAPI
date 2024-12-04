using FluentValidation;

namespace Application.Users.Commands.FavoriteProducts;

public class AddFavoriteCommandValidator: AbstractValidator<AddFavoriteProductCommand>
{
    public AddFavoriteCommandValidator()
    {
        RuleFor(p => p.UserId).NotEmpty();
        RuleFor(p => p.ProductId).NotEmpty();
    }
}