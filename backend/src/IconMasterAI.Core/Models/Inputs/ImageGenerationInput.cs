namespace IconMasterAI.Core.Models.Inputs;

public sealed record ImageGenerationInput(
    string Prompt,
    int NumImages);