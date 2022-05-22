using NoPlan.Api.Endpoints.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Summaries.V1.ToDos;

public sealed class UpdateToDoSummary : Summary<UpdateToDoEndpoint>
{
    public UpdateToDoSummary()
    {
        Summary = "Updates the specified ToDo entity.";
        Description = "Updates the ToDo entity with the provided identifier and data and returns the updated entity";
        Response<ToDoResponse>(200, "Returns the successfully updated ToDo entity");
        Response(404, "The request did not pass validation checks");
        Response(404, "Returned if the specified ToDo entity does not exist");
    }
}
