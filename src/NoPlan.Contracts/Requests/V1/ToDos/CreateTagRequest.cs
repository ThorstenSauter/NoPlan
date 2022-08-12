namespace NoPlan.Contracts.Requests.V1.ToDos;

public sealed record CreateTagRequest
{
    public string Name { get; init; } = null!;
}

public sealed class CreateTagRequestValidator : Validator<CreateTagRequest>
{
    public CreateTagRequestValidator() =>
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tag name is required");
}
