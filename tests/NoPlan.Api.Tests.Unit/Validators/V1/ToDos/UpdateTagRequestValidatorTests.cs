﻿using FluentValidation.TestHelper;
using NoPlan.Contracts.Requests.V1.ToDos;

namespace NoPlan.Api.Tests.Unit.Validators.V1.ToDos;

public sealed class UpdateTagRequestValidatorTests
{
    private readonly UpdateTagRequestValidator _sut = new();

    [Fact]
    public void Validate_ShouldFail_WhenNameIsEmpty()
    {
        // Arrange
        var request = new UpdateTagRequest { Name = string.Empty };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .Only();
    }

    [Fact]
    public void Validate_ShouldPass_WhenNameIsNotEmpty()
    {
        // Arrange
        var request = new UpdateTagRequest { Name = "dotnet" };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
