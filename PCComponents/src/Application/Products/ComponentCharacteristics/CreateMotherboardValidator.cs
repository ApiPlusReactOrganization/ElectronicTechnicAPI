using Domain.Products;
using Domain.Products.PCComponents;
using FluentValidation;

namespace Application.Products.ComponentCharacteristics;

public class CreateMotherboardValidator: AbstractValidator<Motherboard>
{
    public CreateMotherboardValidator()
    {
        RuleFor(x => x.Socket)
            .MinimumLength(3)
            .MaximumLength(255)
            .WithMessage("Socket must be between 3 and 255 characters.");

        RuleFor(x => x.FormFactor)
            .MinimumLength(3)
            .MaximumLength(255)
            .WithMessage("Form factor must be between 3 and 255 characters.");

        RuleFor(x => x.RAMDescription)
            .MinimumLength(3)
            .MaximumLength(1000)
            .WithMessage("RAM description must be between 3 and 1000 characters.");

        RuleFor(x => x.NetworkDescription)
            .MinimumLength(3)
            .MaximumLength(1000)
            .WithMessage("Network description must be between 3 and 1000 characters.");

        RuleFor(x => x.PowerDescription)
            .MinimumLength(3)
            .MaximumLength(1000)
            .WithMessage("Power description must be between 3 and 1000 characters.");

        RuleFor(x => x.AudioDescription)
            .MinimumLength(3)
            .MaximumLength(1000)
            .WithMessage("Audio description must be between 3 and 1000 characters.");

        RuleFor(x => x.ExternalConnectorsDescription)
            .MinimumLength(3)
            .MaximumLength(1000)
            .WithMessage("External connectors description must be between 3 and 1000 characters.");
    }
}