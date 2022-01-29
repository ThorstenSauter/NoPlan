namespace EndpointSamples.Contracts.Requests.ToDos.V1;

public record CreateToDoRequest
{
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
}

public class CreateToDoRequestValidator : Validator<CreateToDoRequest>
{
    public CreateToDoRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("A title for the new ToDo is required")
            .MinimumLength(3).WithMessage("A minimum length of 3 characters is required for the title");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A description for the new ToDo is required");
    }
}
