﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using IconMasterAI.Application.Icons.Commands.GenerateIcon;
using IconMasterAI.Core.Entities;
using IconMasterAI.Core.Errors;
using IconMasterAI.Core.Models.Inputs;
using IconMasterAI.Core.Models.Results;
using IconMasterAI.Core.Repositories;
using IconMasterAI.Core.Services.Icon;
using IconMasterAI.Core.Services.Security;
using IconMasterAI.Core.Shared;
using Moq;

namespace IconMasterAI.Application.UnitTests.Icons.Commands.GenerateIcon;

/*
 *     public async Task<Result<GenerateIconCommandResponse>> Handle(GenerateIconCommand request, CancellationToken ct)
    {
        var user = await _userAccessor.GetUserAsync(ct).ConfigureAwait(false);
        if (user == null)
        {
            return Result.Failure<GenerateIconCommandResponse>(
                DomainErrors.Global.Unauthorized);
        }

        if (!await _userRepository.HasEnoughCreditsAsync(user, request.NumImages, ct))
        {
            return Result.Failure<GenerateIconCommandResponse>(
                DomainErrors.Generator.NotEnoughCredits);
        }

        var input = new IconGenerationInput(
            request.Prompt,
            request.Color,
            request.Style,
            request.NumImages);

        var result = await _iconGenerationService.GenerateIconAsync(user, input, ct).ConfigureAwait(false);
        var response = new GenerateIconCommandResponse(result.Icons);

        return Result.Success(response);
    }
 */

public class GenerateIconCommandHandlerTests
{
    private readonly Mock<IIconGenerationService> _iconGenerationServiceMock;
    private readonly Mock<IUserAccessorService> _userAccessorMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public GenerateIconCommandHandlerTests()
    {
        _iconGenerationServiceMock = new Mock<IIconGenerationService>();
        _userAccessorMock = new Mock<IUserAccessorService>();
        _userRepositoryMock = new Mock<IUserRepository>();
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorized_WhenUserWasNull()
    {
        // Act
        var command = new GenerateIconCommand(
            "prompt",
            "red",
            "flat",
            1);

        var handler = new GenerateIconCommandHandler(
            _iconGenerationServiceMock.Object,
            _userAccessorMock.Object,
            _userRepositoryMock.Object);

        // Arrange
        var result = await handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Global.Unauthorized);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotEnoughCredits_WhenUserDoesNotHaveEnoughCredits()
    {
        // Act
        var command = new GenerateIconCommand(
            "prompt",
            "red",
            "flat",
            1);

        var handler = new GenerateIconCommandHandler(
            _iconGenerationServiceMock.Object,
            _userAccessorMock.Object,
            _userRepositoryMock.Object);

        _userAccessorMock
            .Setup(x => x.GetUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(User.Create(
                "username",
                "email"));

        _userRepositoryMock
            .Setup(x => x.HasEnoughCreditsAsync(
                It.IsAny<User>(), command.NumImages, 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Arrange
        var result = await handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Generator.NotEnoughCredits);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess()
    {
        // Act
        var command = new GenerateIconCommand(
            "prompt",
            "red",
            "flat",
            1);

        var handler = new GenerateIconCommandHandler(
            _iconGenerationServiceMock.Object,
            _userAccessorMock.Object,
            _userRepositoryMock.Object);

        _userAccessorMock
            .Setup(x => x.GetUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(User.Create(
                "username",
                "email"));

        _userRepositoryMock
            .Setup(x => x.HasEnoughCreditsAsync(It.IsAny<User>(), command.NumImages, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _iconGenerationServiceMock
            .Setup(x => x.GenerateIconAsync(
                It.IsAny<User>(),
                It.IsAny<IconGenerationInput>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new IconGenerationResult(new Icon[command.NumImages]));

        // Arrange
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Icons.Length.Should().Be(command.NumImages);
    }
}
