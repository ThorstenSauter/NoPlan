using NoPlan.Contracts.Requests.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Mappers;

public static class TagMappers
{
    public static TagResponse ToResponse(this Tag tag)
    {
        ArgumentNullException.ThrowIfNull(tag);

        return new() { Id = tag.Id, Name = tag.Name, AssignedAt = tag.AssignedAt };
    }

    public static Tag ToEntity(this CreateTagRequest request, DateTime creationTime)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new() { Name = request.Name, AssignedAt = creationTime };
    }

    public static Tag ToEntity(this UpdateTagRequest r, DateTime updateTime)
    {
        ArgumentNullException.ThrowIfNull(r);

        return new() { Id = r.Id, Name = r.Name, AssignedAt = updateTime };
    }
}
