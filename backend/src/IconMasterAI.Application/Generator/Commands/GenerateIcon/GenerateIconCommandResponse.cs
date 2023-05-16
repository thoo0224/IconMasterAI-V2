using IconMasterAI.Core.Entities;

namespace IconMasterAI.Application.Generator.Commands.GenerateIcon;

public sealed record GenerateIconCommandResponse(
    Icon[] Icons);
