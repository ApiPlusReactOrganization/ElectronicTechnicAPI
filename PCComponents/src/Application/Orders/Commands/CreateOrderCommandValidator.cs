using FluentValidation;

namespace Application.Orders.Commands;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x=>x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        
        RuleFor(x=>x.Status).NotEmpty().WithMessage("Status cannot be empty");
        
        RuleFor(x=>x.DeliveryAddress).NotEmpty().WithMessage("DeliveryAddress cannot be empty");
    }
}