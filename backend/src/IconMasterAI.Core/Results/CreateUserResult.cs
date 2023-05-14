namespace IconMasterAI.Core.Results;

public sealed record CreateUserResult(
    bool IsSuccess,
    CreateUserError[]? Errors);

public sealed record CreateUserError(
    string Code,
    string Description);