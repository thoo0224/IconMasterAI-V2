using FluentAssertions;

using IconMasterAI.Application.Users.Commands.Login;
using IconMasterAI.Core.Errors;
using IconMasterAI.Core.Results;
using IconMasterAI.Core.Services.Security;
using IconMasterAI.Core.Shared;

using Moq;

namespace IconMasterAI.Application.UnitTests.Users.Commands;

public class LoginUserCommandHandlerTests
{
    private readonly Mock<IAuthenticationService> _authenticationServiceMock;

    public LoginUserCommandHandlerTests()
    {
        _authenticationServiceMock = new Mock<IAuthenticationService>();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenEmailOrPasswordIsInvalid()
    {
        // Act
        var service = new LoginUserCommandHandler(_authenticationServiceMock.Object);
        var command = new LoginUserCommand("user@email.com", "password");
        var authResult = new AuthenticationResult(
            false, 
            null, 
            DomainErrors.Users.InvalidEmailOrPassword);

        _authenticationServiceMock
            .Setup(x => x.AuthenticateLocalAsync(
            It.IsAny<string>(),
            It.IsAny<string>()))
            .ReturnsAsync(authResult);

        // Arrange
        var result = await service.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(authResult.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenEmailAndPasswordAreRight()
    {
        // Act
        var service = new LoginUserCommandHandler(_authenticationServiceMock.Object);
        var command = new LoginUserCommand("user@email.com", "password");
        var authResult = new AuthenticationResult(true, "token");

        _authenticationServiceMock
            .Setup(x => x.AuthenticateLocalAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(authResult);

        // Arrange
        var result = await service.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Error.Should().Be(Error.None);
    }
}
