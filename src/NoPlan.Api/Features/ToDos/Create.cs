using Microsoft.Identity.Web;
using NoPlan.Api.Services;
using NoPlan.Contracts.Requests.ToDos.V1;
using NoPlan.Contracts.Responses.ToDos.V1;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Features.ToDos;

public class Create : EndpointWithMapping<CreateToDoRequest, ToDoResponse, ToDo>
{
    public IToDoService ToDoService { get; set; } = null!;
    public IDateTimeProvider DateTimeProvider { get; set; } = null!;

    public override void Configure()
    {
        Post("/todos");
        Version(1);
        Policies("User");
        Describe(b => b
            .Accepts<CreateToDoRequest>("application/json")
            .Produces<ToDoResponse>(200, "application/json")
            .WithName("ToDos.Create")
        );

    }

    public override async Task HandleAsync(CreateToDoRequest req, CancellationToken ct)
    {
        var toDo = await ToDoService.CreateAsync(MapToEntity(req));
        await SendCreatedAtAsync<Get>(new { toDo.Id }, MapFromEntity(toDo), ct);
    }

    public override ToDoResponse MapFromEntity(ToDo e) =>
        new() { Id = e.Id, Title = e.Title, Description = e.Description, CreatedAt = e.CreatedAt };

    public override ToDo MapToEntity(CreateToDoRequest r) =>
        new() { Title = r.Title, Description = r.Description, CreatedAt = DateTimeProvider.Now(), CreatedBy = Guid.Parse(User.GetObjectId()!) };
}
