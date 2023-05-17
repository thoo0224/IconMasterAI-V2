namespace IconMasterAI.Core.Models.Inputs;

public sealed record IconGenerationInput(
    string Prompt,
    string Color,
    string Style,
    int NumImages);
