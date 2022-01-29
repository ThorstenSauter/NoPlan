namespace EndpointSamples.Contracts.Requests.ToDos.V1;

public record GetToDoRequest
{
    public Guid Id { get; init; }
}

public class GetToDoRequestValidator : Validator<DeleteToDoRequest>
{
    public GetToDoRequestValidator() =>
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The ToDo identifier is required");
}
