namespace NoPlan.Contracts.Requests.V1.ToDos;

public sealed record GetToDoRequest
{
    /// <summary>
    ///     Gets or initializes the identifier of the object that is requested.
    /// </summary>
    public Guid Id { get; init; }
}

public sealed class GetToDoRequestValidator : Validator<GetToDoRequest>
{
    public GetToDoRequestValidator() =>
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The ToDo identifier is required");
}
