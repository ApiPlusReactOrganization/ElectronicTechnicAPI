using FluentValidation;

namespace Application.Authentications.Commands;

public class CreateUserCommandValidator : AbstractValidator<SignUpCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(u => u.Email)
            .EmailAddress().WithMessage("Invalid mail format")
            .NotEmpty().WithMessage("Enter your email address");
        RuleFor(u => u.Password).NotEmpty().WithMessage("Enter your password");
        RuleFor(u=>u.Name).NotEmpty().WithMessage("Enter your name");
    }
}