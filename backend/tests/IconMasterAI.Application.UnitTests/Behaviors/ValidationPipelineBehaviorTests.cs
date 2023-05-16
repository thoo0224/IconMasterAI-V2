using FluentAssertions;

using FluentValidation;

using IconMasterAI.Application.Behaviors;
using IconMasterAI.Core.Results.Validation;
using IconMasterAI.Core.Shared;

using MediatR;

namespace IconMasterAI.Application.UnitTests.Behaviors;

public class ValidationPipelineBehaviorTests
{
    [Fact]
    public async Task Handle_WithNoValidators_ShouldCallNext()
    {
        // Arrange
        var validators = Enumerable.Empty<IValidator<TestRequest>>();
        var behavior = new ValidationPipelineBehavior<TestRequest, Result>(validators);
        var request = new TestRequest();

        // Act
        var result = await behavior.Handle(request, () => Task.FromResult(Result.Success()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithFailingValidator_ShouldReturnValidationResult()
    {
        // Arrange
        var validators = new[] { new FailingValidator<TestRequest>() };
        var behavior = new ValidationPipelineBehavior<TestRequest, Result>(validators);
        var request = new TestRequest();

        // Act
        var result = await behavior.Handle(request, () => Task.FromResult(Result.Success()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(IValidationResult.Error);
        result.As<ValidationResult>().Errors.Should().NotBeEmpty();
    }
}

public class TestRequest : IRequest<Result>
{
}

public class FailingValidator<TRequest> : AbstractValidator<TRequest>, IValidator<TRequest>
    where TRequest : IRequest<Result>
{
    public FailingValidator()
    {
        RuleFor(x => x).Must(x => false).WithMessage("This validator always fails.");
    }
}
