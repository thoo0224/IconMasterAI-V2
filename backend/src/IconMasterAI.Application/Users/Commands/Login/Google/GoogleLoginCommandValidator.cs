using FluentValidation;

using IconMasterAI.Application.Users.Commands.Register;

namespace IconMasterAI.Application.Users.Commands.Login.Google;

internal class GoogleLoginCommandValidator : AbstractValidator<GoogleLoginCommand>
{
    public GoogleLoginCommandValidator()
    {
        RuleFor(x => x.Credential).NotEmpty();
    }
}
