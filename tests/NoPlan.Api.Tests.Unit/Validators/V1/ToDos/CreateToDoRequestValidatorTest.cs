using FluentValidation.TestHelper;
using NoPlan.Api.Tests.Unit.Fakers;
using NoPlan.Contracts.Requests.V1.ToDos;

namespace NoPlan.Api.Tests.Unit.Validators.V1.ToDos;

public sealed class CreateToDoRequestValidatorTest : TestWithFakes
{
    private readonly CreateToDoRequestValidator _sut;

    public CreateToDoRequestValidatorTest() =>
        _sut = new();

    [Fact]
    public void Validate_ShouldFail_WhenTitleIsEmpty()
    {
        // Arrange
        var request = CreateRequestFaker.Clone()
            .RuleFor(x => x.Title, _ => string.Empty)
            .Generate();

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .Only();
    }

    [Fact]
    public void Validate_ShouldFail_WhenTitleIsTooShort()
    {
        // Arrange
        var request = CreateRequestFaker.Clone()
            .RuleFor(x => x.Title, _ => "ab")
            .Generate();

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .Only();
    }

    [Fact]
    public void Validate_ShouldFail_WhenDescriptionIsEmpty()
    {
        // Arrange
        var request = CreateRequestFaker.Clone()
            .RuleFor(x => x.Description, _ => string.Empty)
            .Generate();

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .Only();
    }

    [Fact]
    public void Validate_ShouldFail_WhenTagsAreNull()
    {
        // Arrange
        var request = CreateRequestFaker.Clone()
            .RuleFor(x => x.Tags, _ => null!)
            .Generate();

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Tags)
            .Only();
    }

    [Fact]
    public void Validate_ShouldPass_WhenRequestIsCompletelyValid()
    {
        // Arrange
        var request = CreateRequestFaker.Generate();

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
