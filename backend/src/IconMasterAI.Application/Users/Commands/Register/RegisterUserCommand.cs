using IconMasterAI.Application.Abstractions.Messaging;

namespace IconMasterAI.Application.Users.Commands.Register;

public sealed record RegisterUserCommand(
    string UserName,
    string Email,
    string Password) : ICommand<RegisterUserCommandResponse>;
