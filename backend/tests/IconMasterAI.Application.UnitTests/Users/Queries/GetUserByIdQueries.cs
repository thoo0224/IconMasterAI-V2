using FluentAssertions;

using IconMasterAI.Application.Users.Queries;
using IconMasterAI.Core.Entities;
using IconMasterAI.Core.Entities.Dto;
using IconMasterAI.Core.Errors;
using IconMasterAI.Core.Repositories;
using IconMasterAI.Core.Services;

using Moq;

namespace IconMasterAI.Application.UnitTests.Users.Queries;

public class GetUserByIdQueries
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMappingService> _mappingServiceMock;

    public GetUserByIdQueries()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _mappingServiceMock = new Mock<IMappingService>();
    }

    [Fact]
    public async Task Handle_ReturnNull_WhenUserWasNotFound()
    {
        // Act
        var command = new GetUserByIdQuery("invalid id");
        var handler = new GetUserByIdQueryHandler(
            _userRepositoryMock.Object,
            _mappingServiceMock.Object);

        _userRepositoryMock
            .Setup(x => x.FindUserById(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        // Arrange
        var result = await handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Users.UserNotFound);
        result.Value.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ReturnSuccess_WhenUserWasFound()
    {
        // Act
        var user = User.Create(
            "thoo0224",
            "test@example.com",
            "avatar.com");

        var userDto = new UserDto
        {
            Email = user.Email,
            AvatarUrl = user.Email,
            UserName = user.Email
        };

        var command = new GetUserByIdQuery("id");
        var handler = new GetUserByIdQueryHandler(
            _userRepositoryMock.Object,
            _mappingServiceMock.Object);

        _userRepositoryMock
            .Setup(x => x.FindUserById(It.IsAny<string>()))
            .ReturnsAsync(user);

        _mappingServiceMock
            .Setup(x => x.MapToDto(It.IsAny<User>()))
            .Returns(userDto);

        // Arrange
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(userDto);
    }
}
