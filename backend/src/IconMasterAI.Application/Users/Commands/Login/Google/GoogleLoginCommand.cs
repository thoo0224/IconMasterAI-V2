using IconMasterAI.Application.Abstractions.Messaging;

namespace IconMasterAI.Application.Users.Commands.Login.Google;

public sealed record GoogleLoginCommand(
    string Credential) : ICommand<LoginUserCommandResponse>;