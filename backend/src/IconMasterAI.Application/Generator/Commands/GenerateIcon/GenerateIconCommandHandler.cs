using IconMasterAI.Application.Abstractions.Messaging;
using IconMasterAI.Core.Models.Inputs;
using IconMasterAI.Core.Services.Icon;
using IconMasterAI.Core.Shared;

namespace IconMasterAI.Application.Generator.Commands.GenerateIcon;

public class GenerateIconCommandHandler
    : ICommandHandler<GenerateIconCommand, GenerateIconCommandResponse>
{
    private readonly IIconGenerationService _iconGenerationService;

    public GenerateIconCommandHandler(IIconGenerationService iconGenerationService)
    {
        _iconGenerationService = iconGenerationService;
    }

    public async Task<Result<GenerateIconCommandResponse>> Handle(GenerateIconCommand request, CancellationToken ct)
    {
        var input = new IconGenerationInput(
            request.Prompt,
            request.Color,
            request.Style);

        var result = await _iconGenerationService.GenerateIconAsync(input, ct).ConfigureAwait(false);
        var response = new GenerateIconCommandResponse(result.Icons);

        return Result.Success(response);
    }
}
