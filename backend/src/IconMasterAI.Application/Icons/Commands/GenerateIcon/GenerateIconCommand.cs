using IconMasterAI.Application.Abstractions.Messaging;

namespace IconMasterAI.Application.Icons.Commands.GenerateIcon;

public sealed record GenerateIconCommand(
    string Prompt,
    string Color,
    string Style) : ICommand<GenerateIconCommandResponse>;