namespace NoPlan.Contracts.Requests.V1.ToDos;

public sealed record UpdateToDoRequest
{
    /// <summary>
    ///     Gets or initializes the identifier of the object to update.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     Gets or initializes the updated title. Must be at least 3 characters long.
    /// </summary>
    public string Title { get; init; } = null!;

    /// <summary>
    ///     Gets or initializes the updated description. Cannot be null or empty.
    /// </summary>
    public string Description { get; init; } = null!;

    /// <summary>
    ///     Gets or initializes the required list of tags to be associated with the new entity.
    /// </summary>
    public ICollection<UpdateTagRequest> Tags { get; init; } = null!;
}

public sealed class UpdateToDoRequestValidator : Validator<UpdateToDoRequest>
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

        RuleFor(x => x.Tags)
            .NotNull().WithMessage("The list of associated tags is required");

        RuleForEach(x => x.Tags)
            .SetValidator(new UpdateTagRequestValidator());
    }
}
