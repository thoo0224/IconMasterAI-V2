using IconMasterAI.Core.Shared;

namespace IconMasterAI.Core.Results.Validation;

public interface IValidationResult
{
    public static readonly Error Error = new(
        "ValidationError",
        "A validation problem occurred.");

    public Error[] Errors { get; }
}
