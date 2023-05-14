using FluentValidation;

namespace IconMasterAI.Application.Users.Commands.Register;

internal class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(16);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MaximumLength(32)
            .WithMessage("Password must be at most 32 characters long")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long")
            .Must(m => m.Any(c => !char.IsLetterOrDigit(c)))
            .WithMessage("Password must contain at least one non-alphanumeric character")
            .Must(m => m.Any(char.IsDigit))
            .WithMessage("Password must contain at least one digit");
    }
}
