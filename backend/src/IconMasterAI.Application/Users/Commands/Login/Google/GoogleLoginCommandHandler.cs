using IconMasterAI.Application.Abstractions.Messaging;
using IconMasterAI.Core.Services.Security;
using IconMasterAI.Core.Shared;

namespace IconMasterAI.Application.Users.Commands.Login.Google;

// TODO: Write tests
internal sealed class GoogleLoginCommandHandler
    : ICommandHandler<GoogleLoginCommand, LoginUserCommandResponse>
{
    private readonly IAuthenticationService _authenticationService;

    public GoogleLoginCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<Result<LoginUserCommandResponse>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.AuthenticateWithGoogleAsync(
            request.Credential).ConfigureAwait(false);

        if (!result.IsSuccess)
        {
            return Result.Failure<LoginUserCommandResponse>(result.Error!);
        }

        var response = new LoginUserCommandResponse(
            result.Token!);

        return Result.Success(response);
    }
}
