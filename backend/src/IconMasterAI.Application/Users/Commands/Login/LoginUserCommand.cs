using IconMasterAI.Application.Abstractions.Messaging;

namespace IconMasterAI.Application.Users.Commands.Login;

public sealed record LoginUserCommand(
    string Email,
    string Password) : ICommand<LoginUserCommandResponse>;