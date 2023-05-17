using IconMasterAI.Core.Entities;
using IconMasterAI.Core.Models.Inputs;
using IconMasterAI.Core.Models.Results;
using IconMasterAI.Core.Repositories;
using IconMasterAI.Core.Services;
using IconMasterAI.Core.Services.Icon;

namespace IconMasterAI.Infrastructure.Services;

internal sealed class IconGenerationService : IIconGenerationService
{
    private readonly IImageGenerationService _imageGenerationService;
    private readonly IUserRepository _userRepository;
    private readonly IIconRepository _iconRepository;
    private readonly IUnitOfWork _unitOfWork;

    public IconGenerationService(
        IImageGenerationService imageGenerationService, 
        IUserRepository userRepository, 
        IIconRepository iconRepository, 
        IUnitOfWork unitOfWork)
    {
        _imageGenerationService = imageGenerationService;
        _userRepository = userRepository;
        _iconRepository = iconRepository;
        _unitOfWork = unitOfWork;
    }

    // TODO: Prompt Enhancer, IPromptBuilderService, Style classes, Proper error handling
    public async Task<IconGenerationResult> GenerateIconAsync(
        User user,
        IconGenerationInput body,
        CancellationToken ct = default)
    {
        var originalPrompt = body.Prompt;
        var finalPrompt =
            $"a modern {body.Style} icon in {body.Color} of {originalPrompt}, rounded, minimalistic, high quality, trending on art station, unreal engine graphics quality";
        var input = new ImageGenerationInput(
            finalPrompt, 1);

        var generationResult = await _imageGenerationService.CreateImageAsync(input, ct).ConfigureAwait(false);
        var icons = await _iconRepository.CreateManyAsync(
            originalPrompt,
            finalPrompt,
            generationResult.Images, ct).ConfigureAwait(false);

        await _userRepository.RemoveCreditsAsync(user, body.NumImages, ct).ConfigureAwait(false);
        await _unitOfWork.SaveChangedAsync(ct).ConfigureAwait(false);

        var result = new IconGenerationResult(icons);
        return result;
    }
}
