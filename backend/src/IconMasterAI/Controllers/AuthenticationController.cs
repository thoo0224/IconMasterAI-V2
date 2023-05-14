using IconMasterAI.Abstractions;
using IconMasterAI.Application.Users.Commands.Login;
using IconMasterAI.Application.Users.Commands.Login.Google;
using IconMasterAI.Application.Users.Commands.Register;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace IconMasterAI.Controllers;

[Route("api/auth")]
public class AuthenticationController : ApiController
{
    private readonly IConfiguration _configuration;

    public AuthenticationController(ISender sender, IConfiguration configuration) 
        : base(sender)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromForm] LoginUserCommand command, CancellationToken ct)
    {
        var result = await Sender.Send(command, ct).ConfigureAwait(false);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpPost("login/external/google")]
    public async Task<IActionResult> LoginWithGoogleAsync([FromForm] GoogleLoginCommand command, CancellationToken ct)
    {
        var result = await Sender.Send(command, ct).ConfigureAwait(false);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Redirect(
            $"{_configuration["AppSettings:FrontendUrl"]}/login?accessToken={result.Value?.Token ?? "None"}");
    }

    [HttpPost("register")]
    public async Task<IActionResult> CreateUserAsync([FromForm] RegisterUserCommand command, CancellationToken ct)
    {
        var result = await Sender.Send(command, ct).ConfigureAwait(false);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
}
