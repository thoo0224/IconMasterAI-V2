using FluentAssertions;

using IconMasterAI.Application.Users.Commands.Register;
using IconMasterAI.Core.Entities;
using IconMasterAI.Core.Errors;
using IconMasterAI.Core.Repositories;
using IconMasterAI.Core.Results;

using Moq;

namespace IconMasterAI.Application.UnitTests.Users.Commands;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public RegisterUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenEmailIsNotUnique()
    {
        // Act
        var command = new RegisterUserCommand(
            "username",
            "email@test.com",
            "password");

        _userRepositoryMock.Setup(
                x => x.IsEmailUniqueAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        var handler = new RegisterUserCommandHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Arrange
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(DomainErrors.Users.EmailAlreadyInUse);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenUserNameIsNotUnique()
    {
        // Act
        var command = new RegisterUserCommand(
            "username",
            "email@test.com",
            "password");

        _userRepositoryMock.Setup(
                x => x.IsEmailUniqueAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _userRepositoryMock.Setup(
                x => x.IsUserNameUniqueAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        var handler = new RegisterUserCommandHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Arrange
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(DomainErrors.Users.UserNameAlreadyInUse);
    }

    [Fact]
    public async Task Handle_Should_CreateUser_WhenEmailAndUserNameIsUnique()
    {
        // Act
        var command = new RegisterUserCommand(
            "username",
            "email@test.com",
            "password");

        _userRepositoryMock.Setup(
                x => x.IsEmailUniqueAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _userRepositoryMock.Setup(
                x => x.IsUserNameUniqueAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _userRepositoryMock.Setup(
                x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(new CreateUserResult(true, null));

        var handler = new RegisterUserCommandHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Arrange
        await handler.Handle(command, default);

        // Assert
        _userRepositoryMock.Verify(
            x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenEmailAndUserNameIsUnique()
    {
        // Act
        var command = new RegisterUserCommand(
            "username",
            "email@test.com",
            "password");

        _userRepositoryMock.Setup(
                x => x.IsEmailUniqueAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _userRepositoryMock.Setup(
                x => x.IsUserNameUniqueAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _userRepositoryMock.Setup(
                x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(new CreateUserResult(true, null));

        var handler = new RegisterUserCommandHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Arrange
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
}
