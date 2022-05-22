using NoPlan.Api.Endpoints.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Summaries.V1.ToDos;

public sealed class GetToDoSummary : Summary<GetToDoEndpoint>
{
    public GetToDoSummary()
    {
        Summary = "Retrieves the specified ToDo entity";
        Description = "Retrieves the ToDo entity with the provided identifier and returns it";
        Response<ToDoResponse>(200, "Returns the ToDo entity");
        Response(404, "The specified ToDo entity does not exist");
    }
}
