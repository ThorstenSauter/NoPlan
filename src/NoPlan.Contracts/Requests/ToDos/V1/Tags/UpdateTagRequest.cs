namespace NoPlan.Contracts.Requests.ToDos.V1.Tags;

public record UpdateTagRequest
{
    public string Name { get; init; } = null!;
}

public class UpdateTagRequestValidator : Validator<UpdateTagRequest>
{
    public UpdateTagRequestValidator() =>
        RuleFor(x => x.Name).NotEmpty().WithMessage("Tag name is required");
}
