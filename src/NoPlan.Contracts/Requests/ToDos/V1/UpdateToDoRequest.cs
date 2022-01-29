namespace EndpointSamples.Contracts.Requests.ToDos.V1;

public record UpdateToDoRequest
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
}

public class UpdateToDoRequestValidator : Validator<UpdateToDoRequest>
{
    public UpdateToDoRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The ToDo identifier is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("A title for the ToDo to update is required")
            .MinimumLength(3).WithMessage("A minimum length of 3 characters is required for new title");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A description for the ToDo to update is required");
    }
}
