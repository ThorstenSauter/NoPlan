using NoPlan.Api.Endpoints.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Summaries.V1.ToDos;

public sealed class DeleteToDoSummary : Summary<DeleteToDoEndpoint>
{
    public DeleteToDoSummary()
    {
        Summary = "Deletes the specified ToDo entity";
        Description = "Deletes the ToDo entity with the provided identifier and returns the deleted data";
        Response<ToDoResponse>(200, "Returns the successfully deleted ToDo entity");
        Response(404, "Returned if the specified ToDo entity did not exist");
    }
}
