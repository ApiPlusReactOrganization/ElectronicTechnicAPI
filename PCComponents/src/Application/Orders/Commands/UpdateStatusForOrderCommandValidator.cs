using FluentValidation;

namespace Application.Orders.Commands;

public class UpdateStatusForOrderCommandValidator : AbstractValidator<UpdateStatusForOrderCommand>
{
    public UpdateStatusForOrderCommandValidator()
    {
        RuleFor(x=>x.OrderId).NotEmpty().WithMessage("Order Id cannot be empty");
        
        RuleFor(x=>x.StatusId).NotEmpty().WithMessage("Status Id cannot be empty");
    }
}