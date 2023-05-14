using IconMasterAI.Abstractions;
using IconMasterAI.Application.Users.Queries;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IconMasterAI.Controllers;

[Route("api/user")]
public class UserController : ApiController
{
    public UserController(ISender sender) 
        : base(sender)
    {
    }

    [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetUserAsync(CancellationToken ct)
    {
        var user = await ResolveUserAsync().ConfigureAwait(false);
        if (user == null)
        {
            return Unauthorized();
        }

        var query = new GetUserByIdQuery(user.Id);
        var result = await Sender.Send(query, ct).ConfigureAwait(false);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
}
