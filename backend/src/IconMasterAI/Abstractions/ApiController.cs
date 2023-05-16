using System.Security.Claims;

using IconMasterAI.Core.Entities;
using IconMasterAI.Core.Repositories;
using IconMasterAI.Core.Results.Validation;
using IconMasterAI.Core.Shared;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace IconMasterAI.Abstractions;

[ApiController]
public abstract class ApiController : ControllerBase
{
    protected readonly ISender Sender;

    protected ApiController(ISender sender)
    {
        Sender = sender;
    }

    protected IActionResult HandleFailure(Result result)
        => result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult =>
                BadRequest(
                    CreateProblemDetails(
                        "Validation Error", StatusCodes.Status400BadRequest,
                        result.Error, validationResult.Errors)),
            _ =>
                BadRequest(
                    CreateProblemDetails(
                        "Bad Request", StatusCodes.Status400BadRequest,
                        result.Error))
        };

    public static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null)
        => new()
        {
            Title = title,
            Status = status,
            Type = error.Code,
            Detail = error.Message,
            Extensions = { { nameof(errors), errors } }
        };

    public async Task<User?> ResolveUserAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return null;
        }

        var userRepository = HttpContext.RequestServices.GetRequiredService<IUserRepository>();
        var user = await userRepository.FindUserByIdAsync(userId)
            .ConfigureAwait(false);

        return user;
    }
}
