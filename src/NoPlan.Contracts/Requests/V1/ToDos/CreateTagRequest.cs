namespace NoPlan.Contracts.Requests.V1.ToDos;

public record CreateTagRequest
{
    public string Name { get; init; } = null!;
}

public class CreateTagRequestValidator : Validator<CreateTagRequest>
{
    public CreateTagRequestValidator() =>
        RuleFor(x => x.Name).NotEmpty().WithMessage("Tag name is required");
}
