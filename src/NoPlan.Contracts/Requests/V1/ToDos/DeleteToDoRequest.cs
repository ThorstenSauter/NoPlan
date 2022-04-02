﻿using Microsoft.AspNetCore.Mvc;

namespace NoPlan.Contracts.Requests.V1.ToDos;

public record DeleteToDoRequest
{
    /// <summary>
    ///     The identifier of the ToDo object to delete.
    /// </summary>
    [FromRoute]
    public Guid Id { get; init; }
}

public class DeleteToDoRequestValidator : Validator<DeleteToDoRequest>
{
    public DeleteToDoRequestValidator() =>
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The ToDo identifier is required");
}