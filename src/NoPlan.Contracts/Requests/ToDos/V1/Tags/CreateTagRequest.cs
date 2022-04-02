namespace NoPlan.Contracts.Requests.ToDos.V1.Tags;

public record CreateTagRequest
{
    public string Name { get; init; } = null!;
}

public class CreateTagRequestValidator : Validator<CreateTagRequest>
{
    public CreateTagRequestValidator() =>
        RuleFor(x => x.Name).NotEmpty().WithMessage("Tag name is required");
}
