using Microsoft.AspNetCore.Mvc;

namespace NoPlan.Contracts.Requests.V1.ToDos;

public sealed record DeleteToDoRequest
{
    /// <summary>
    ///     Gets or initializes the identifier of the object to delete.
    /// </summary>
    [FromRoute]
    public Guid Id { get; init; }
}

public sealed class DeleteToDoRequestValidator : Validator<DeleteToDoRequest>
{
    public DeleteToDoRequestValidator() =>
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The ToDo identifier is required");
}
