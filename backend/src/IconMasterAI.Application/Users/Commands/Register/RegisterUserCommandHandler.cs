using IconMasterAI.Application.Abstractions.Messaging;
using IconMasterAI.Core.Entities;
using IconMasterAI.Core.Errors;
using IconMasterAI.Core.Repositories;
using IconMasterAI.Core.Shared;

namespace IconMasterAI.Application.Users.Commands.Register;

internal sealed class RegisterUserCommandHandler
    : ICommandHandler<RegisterUserCommand, RegisterUserCommandResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RegisterUserCommandResponse>> Handle(RegisterUserCommand command, CancellationToken ct)
    {
        if (!await _userRepository.IsEmailUniqueAsync(command.Email))
            return Result.Failure<RegisterUserCommandResponse>(DomainErrors.Users.EmailAlreadyInUse);

        if (!await _userRepository.IsUserNameUniqueAsync(command.UserName))
            return Result.Failure<RegisterUserCommandResponse>(DomainErrors.Users.UserNameAlreadyInUse);

        var user = User.Create(
            command.UserName,
            command.Email);

        var result = await _userRepository.CreateAsync(
            user, command.Password).ConfigureAwait(false);
        if (!result.IsSuccess)
            return Result.Failure<RegisterUserCommandResponse>(DomainErrors.Users.IdentityError);

        var response = new RegisterUserCommandResponse(user.Id);
        return Result.Success(response);
    }
}
