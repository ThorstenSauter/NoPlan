namespace NoPlan.Contracts.Requests.V1.ToDos;

public sealed record UpdateTagRequest
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;
}

public sealed class UpdateTagRequestValidator : Validator<UpdateTagRequest>
{
    public UpdateTagRequestValidator() =>
        RuleFor(x => x.Name).NotEmpty().WithMessage("Tag name is required");
}
