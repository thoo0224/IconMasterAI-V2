using IconMasterAI.Core.Models.Inputs;
using IconMasterAI.Core.Models.Results;

namespace IconMasterAI.Core.Services;

public interface IImageGenerationService
{
    Task<ImageGenerationResult> CreateImageAsync(ImageGenerationInput input, CancellationToken ct = default);
}
