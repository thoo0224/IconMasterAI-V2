using IconMasterAI.Application.Abstractions.Messaging;
using IconMasterAI.Core.Errors;
using IconMasterAI.Core.Models.Inputs;
using IconMasterAI.Core.Repositories;
using IconMasterAI.Core.Services.Icon;
using IconMasterAI.Core.Services.Security;
using IconMasterAI.Core.Shared;

namespace IconMasterAI.Application.Icons.Commands.GenerateIcon;

public class GenerateIconCommandHandler
    : ICommandHandler<GenerateIconCommand, GenerateIconCommandResponse>
{
    private readonly IIconGenerationService _iconGenerationService;
    private readonly IUserAccessorService _userAccessor;
    private readonly IUserRepository _userRepository;

    public GenerateIconCommandHandler(
        IIconGenerationService iconGenerationService, 
        IUserAccessorService userAccessor, 
        IUserRepository userRepository)
    {
        _iconGenerationService = iconGenerationService;
        _userAccessor = userAccessor;
        _userRepository = userRepository;
    }

    public async Task<Result<GenerateIconCommandResponse>> Handle(GenerateIconCommand request, CancellationToken ct)
    {
        var user = await _userAccessor.GetUserAsync(ct).ConfigureAwait(false);
        if (user == null)
        {
            return Result.Failure<GenerateIconCommandResponse>(
                DomainErrors.Global.Unauthorized);
        }

        if (!await _userRepository.HasEnoughCreditsAsync(user, request.NumImages, ct))
        {
            return Result.Failure<GenerateIconCommandResponse>(
                DomainErrors.Generator.NotEnoughCredits);
        }

        var input = new IconGenerationInput(
            request.Prompt,
            request.Color,
            request.Style,
            request.NumImages);

        var result = await _iconGenerationService.GenerateIconAsync(user, input, ct).ConfigureAwait(false);
        var response = new GenerateIconCommandResponse(result.Icons);

        return Result.Success(response);
    }
}
