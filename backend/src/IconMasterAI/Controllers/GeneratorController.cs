using IconMasterAI.Abstractions;
using IconMasterAI.Application.Icons.Commands.GenerateIcon;
using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IconMasterAI.Controllers;

[Route("api/generate")]
public class GeneratorController : ApiController
{
    public GeneratorController(ISender sender) 
        : base(sender)
    {
    }

    [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GenerateAsync([FromForm] GenerateIconCommand command, CancellationToken ct)
    {
        var result = await Sender.Send(command, ct).ConfigureAwait(false);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
}
