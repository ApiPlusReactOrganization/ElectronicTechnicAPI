using FluentValidation;

namespace Application.Users.Commands;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(u => u.Email)
            .EmailAddress().WithMessage("Invalid mail format")
            .NotEmpty().WithMessage("Enter your email address");
        
        RuleFor(u=>u.UserName).NotEmpty().WithMessage("Enter your name");
    }
}