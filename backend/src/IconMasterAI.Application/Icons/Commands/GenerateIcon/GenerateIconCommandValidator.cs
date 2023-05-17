using FluentValidation;

namespace IconMasterAI.Application.Icons.Commands.GenerateIcon;

internal sealed class GenerateIconCommandValidator 
    : AbstractValidator<GenerateIconCommand>
{
    public GenerateIconCommandValidator()
    {
        RuleFor(x => x.Prompt)
            .MaximumLength(1000)
            .WithMessage("Prompt can't be longer than 1000 characters.");

        RuleFor(x => x.NumImages)
            .InclusiveBetween(1, 10)
            .WithMessage("The number of icons to generate must be between 1 and 10.");

        // TODO: Rule for color
        // TODO: Rule for style
    }
}
