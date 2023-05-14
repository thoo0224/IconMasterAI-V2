using IconMasterAI.Application.Abstractions.Messaging;
using IconMasterAI.Core.Shared;

namespace IconMasterAI.Application.Generator.Commands.GenerateIcon;

public class GenerateIconCommandHandler
    : ICommandHandler<GenerateIconCommand, GenerateIconCommandResponse>
{
    public Task<Result<GenerateIconCommandResponse>> Handle(GenerateIconCommand request, CancellationToken ct)
    {
        return Task.FromResult(Result.Success(new GenerateIconCommandResponse(string.Empty)));
    }
}
