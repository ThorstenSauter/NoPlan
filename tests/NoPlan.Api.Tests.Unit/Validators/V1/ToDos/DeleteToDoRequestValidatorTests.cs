using FluentValidation.TestHelper;
using NoPlan.Contracts.Requests.V1.ToDos;

namespace NoPlan.Api.Tests.Unit.Validators.V1.ToDos;

public sealed class DeleteToDoRequestValidatorTests
{
    private readonly DeleteToDoRequestValidator _sut = new();

    [Fact]
    public void Validate_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var request = new DeleteToDoRequest { Id = Guid.Empty };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
        result.ShouldHaveAnyValidationError();
    }

    [Fact]
    public void Validate_ShouldPass_WhenIdIsNotEmpty()
    {
        // Arrange
        var request = new DeleteToDoRequest { Id = Guid.NewGuid() };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
