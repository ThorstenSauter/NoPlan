namespace NoPlan.Contracts.Requests.V1.ToDos;

public record GetToDoRequest
{
    /// <summary>
    ///     The identifier of the ToDo object that is requested.
    /// </summary>
    public Guid Id { get; init; }
}

public class GetToDoRequestValidator : Validator<GetToDoRequest>
{
    public GetToDoRequestValidator() =>
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The ToDo identifier is required");
}
