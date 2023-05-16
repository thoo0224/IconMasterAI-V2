using IconMasterAI.Core.Entities;

namespace IconMasterAI.Application.Icons.Commands.GenerateIcon;

public sealed record GenerateIconCommandResponse(
    Icon[] Icons);
