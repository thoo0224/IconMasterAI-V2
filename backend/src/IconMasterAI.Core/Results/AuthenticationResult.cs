using IconMasterAI.Core.Shared;

namespace IconMasterAI.Core.Results;

public sealed record AuthenticationResult(
    bool IsSuccess,
    string? Token,
    Error? Error = null)
{
    public static AuthenticationResult Failure(Error error) 
        => new(false, null, error);

    public static AuthenticationResult Success(string token)
        => new(true, token, null);
}