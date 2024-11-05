using FluentValidation;

namespace Application.Manufacturers.Commands;

public class UpdateFacultyCommandValidator : AbstractValidator<UpdateManufacturerCommand>
{
    public UpdateFacultyCommandValidator()
    {
        RuleFor(x => x.ManufacturerId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255).MinimumLength(3);
    }
}