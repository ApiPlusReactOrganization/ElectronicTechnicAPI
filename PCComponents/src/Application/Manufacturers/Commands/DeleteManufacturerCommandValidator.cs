using FluentValidation;

namespace Application.Manufacturers.Commands;

public class DeleteManufacturerCommandValidator: AbstractValidator<DeleteManufacturerCommand>
{
    public DeleteManufacturerCommandValidator()
    {
        RuleFor(x => x.ManufacturerId)
            .NotEmpty()
            .WithMessage("Manufacturer ID is required");
    }
}