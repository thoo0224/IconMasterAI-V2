using AutoMapper;

using FluentAssertions;

using IconMasterAI.Core.Entities;
using IconMasterAI.Core.Entities.Dto;
using IconMasterAI.Persistence.Services;

using Moq;

namespace IconMasterAI.Persistence.UnitTests.Services;

public class MappingServiceTests
{
    private readonly Mock<IMapper> _mapperMock;

    public MappingServiceTests()
    {
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public void MapToDto_ShouldReturnCorrectDto()
    {
        // Act
        var user = User.Create(
            "username",
            "test@test.com",
            "avatar.com");

        var dto = new UserDto
        {
            UserName = user.UserName,
            Email = user.UserName,
            AvatarUrl = user.UserName,
        };

        var service = new MappingService(
            _mapperMock.Object);

        _mapperMock
            .Setup(x => x.Map<UserDto>(It.IsAny<User>()))
            .Returns(dto);

        // Arrange
        var result = service.MapToDto(user);

        // Assert
        result.Should().Be(dto);
    }
}
