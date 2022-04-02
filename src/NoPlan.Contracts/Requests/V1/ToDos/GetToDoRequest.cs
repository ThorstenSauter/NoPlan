namespace NoPlan.Contracts.Requests.V1.ToDos;

public sealed record GetToDoRequest
{
    /// <summary>
    ///     The identifier of the ToDo object that is requested.
    /// </summary>
    public Guid Id { get; init; }
}

public sealed class GetToDoRequestValidator : Validator<GetToDoRequest>
{
    public GetToDoRequestValidator() =>
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The ToDo identifier is required");
}
