namespace NoPlan.Contracts.Requests.ToDos.V1;

public record CreateToDoRequest
{
    /// <summary>
    ///     The ToDo title. Must be at least 3 characters long.
    /// </summary>
    public string Title { get; init; } = null!;

    /// <summary>
    ///     The ToDo description. Cannot be null or empty.
    /// </summary>
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
