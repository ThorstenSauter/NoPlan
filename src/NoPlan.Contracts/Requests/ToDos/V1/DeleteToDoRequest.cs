using Microsoft.AspNetCore.Mvc;

namespace EndpointSamples.Contracts.Requests.ToDos.V1;

public record DeleteToDoRequest
{
    [FromRoute]
    public Guid Id { get; init; }
}

public class DeleteToDoRequestValidator : Validator<DeleteToDoRequest>
{
    public DeleteToDoRequestValidator() =>
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The ToDo identifier is required");
}
