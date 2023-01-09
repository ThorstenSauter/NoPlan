namespace NoPlan.Contracts.Requests.V1.ToDos;

public sealed record CreateToDoRequest
{
    /// <summary>
    ///     Gets or initializes the title. Must be at least 3 characters long.
    /// </summary>
    public string Title { get; init; } = null!;

    /// <summary>
    ///     Gets or initializes the description. Cannot be null or empty.
    /// </summary>
    public string Description { get; init; } = null!;

    /// <summary>
    ///     Gets or initializes the required list of tags that should be associated with the new entity.
    /// </summary>
    public ICollection<CreateTagRequest> Tags { get; init; } = null!;
}

public sealed class CreateToDoRequestValidator : Validator<CreateToDoRequest>
{
    public CreateToDoRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("A title for the new ToDo is required")
            .MinimumLength(3).WithMessage("A minimum length of 3 characters is required for the title");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A description for the new ToDo is required");

        RuleFor(x => x.Tags)
            .NotNull().WithMessage("The list of associated tags is required");

        RuleForEach(x => x.Tags)
            .SetValidator(new CreateTagRequestValidator());
    }
}
