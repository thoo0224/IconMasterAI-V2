using FluentAssertions;
using IconMasterAI.Core.Entities;
using IconMasterAI.Core.Errors;
using IconMasterAI.Core.Repositories;
using IconMasterAI.Core.Services.Security;
using IconMasterAI.Infrastructure.Services.Security;

using Moq;

namespace IconMasterAI.Infrastructure.UnitTests.Services.Security;

// TODO: Write tests for AuthenticateWithGoogleAsync
public class AuthenticationServiceTests
{
    private readonly Mock<IIdentityService> _identityServiceMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IJwtService> _jwtServiceMock;

    public AuthenticationServiceTests()
    {
        _identityServiceMock = new Mock<IIdentityService>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _jwtServiceMock = new Mock<IJwtService>();
    }

    [Fact]
    public async Task AuthenticateLocalAsync_Should_ReturnFailure_WhenEmailIsInvalid()
    {
        // Act
        var service = new AuthenticationService(
            _identityServiceMock.Object,
            _userRepositoryMock.Object,
            _jwtServiceMock.Object);

        _userRepositoryMock
            .Setup(x => x.FindUserByEmail("email@test.com"))
            .ReturnsAsync((User?)null);

        // Arrange
        var result = await service.AuthenticateLocalAsync("email@test.com", "password");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(DomainErrors.Users.InvalidEmailOrPassword);
    }

    [Fact]
    public async Task AuthenticateLocalAsync_Should_ReturnFailure_WhenPasswordIsInvalid()
    {
        // Act
        var service = new AuthenticationService(
            _identityServiceMock.Object,
            _userRepositoryMock.Object,
            _jwtServiceMock.Object);

        _userRepositoryMock
            .Setup(x => x.FindUserByEmail("email@test.com"))
            .ReturnsAsync(User.Create("username", "email@test.com"));

        _identityServiceMock
            .Setup(x => x.CheckPasswordSignInAsync(It.IsAny<User>(), "password"))
            .ReturnsAsync(false);

        // Arrange
        var result = await service.AuthenticateLocalAsync("email@test.com", "password");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(DomainErrors.Users.InvalidEmailOrPassword);
    }

    [Fact]
    public async Task AuthenticateLocalAsync_Should_ReturnSuccess_WhenCredentialsAreCorrect()
    {
        // Act
        var service = new AuthenticationService(
            _identityServiceMock.Object,
            _userRepositoryMock.Object,
            _jwtServiceMock.Object);

        _userRepositoryMock
            .Setup(x => x.FindUserByEmail("email@test.com"))
            .ReturnsAsync(User.Create("username", "email@test.com"));

        _identityServiceMock
            .Setup(x => x.CheckPasswordSignInAsync(It.IsAny<User>(), "password"))
            .ReturnsAsync(true);

        _jwtServiceMock
            .Setup(x => x.CreateToken(It.IsAny<User>()))
            .Returns("token");

        // Arrange
        var result = await service.AuthenticateLocalAsync("email@test.com", "password");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Token.Should().NotBeEmpty();
    }
}
