using IconMasterAI.Application.Abstractions.Messaging;
using IconMasterAI.Core.Services.Security;
using IconMasterAI.Core.Shared;

namespace IconMasterAI.Application.Users.Commands.Login;

// TODO: Write tests
internal sealed class LoginUserCommandHandler
    : ICommandHandler<LoginUserCommand, LoginUserCommandResponse>
{
    private readonly IAuthenticationService _authenticationService;

    public LoginUserCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<Result<LoginUserCommandResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.AuthenticateLocalAsync(
            request.Email,
            request.Password).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            return Result.Failure<LoginUserCommandResponse>(result.Error!);
        }

        var response = new LoginUserCommandResponse(
            result.Token!);

        return Result.Success(response);
    }
}
