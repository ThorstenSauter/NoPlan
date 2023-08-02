using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Mappers;

public static class ToDoMappers
{
    public static ToDoResponse ToResponse(this ToDo toDo)
    {
        ArgumentNullException.ThrowIfNull(toDo);

        return new()
        {
            Id = toDo.Id,
            Title = toDo.Title,
            Description = toDo.Description,
            Tags = toDo.Tags.Select(t => t.ToResponse()),
            CreatedAt = toDo.CreatedAt
        };
    }

    public static ToDosResponse ToResponse(this IEnumerable<ToDo> toDos) =>
        new() { ToDos = toDos.Select(t => t.ToResponse()) };

    public static ToDo ToEntity(this UpdateToDoRequest request, DateTime updateTime, Guid creatorId)
    {
        ArgumentNullException.ThrowIfNull(request);
        return new()
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            Tags = request.Tags.Select(tagRequest => tagRequest.ToEntity(updateTime)).ToList(),
            CreatedBy = creatorId
        };
    }
}
