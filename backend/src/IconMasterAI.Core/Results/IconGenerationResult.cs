namespace IconMasterAI.Core.Results;

public sealed record IconGenerationResult(
    bool IsSuccess,
    string[]? Urls)
{
    public static IconGenerationResult Failure()
        => new(false, null);

    public static IconGenerationResult Success(string[] value)
        => new(true, value);
}