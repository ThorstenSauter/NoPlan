using NoPlan.Api.Endpoints.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Summaries.V1.ToDos;

public sealed class GetAllToDosSummary : Summary<GetAllToDosEndpoint>
{
    public GetAllToDosSummary()
    {
        Summary = "Retrieves all ToDo entities";
        Description = "Retrieves all ToDo entities and returns them";
        Response<ToDosResponse>(200, "Returns all ToDo entities");
    }
}
