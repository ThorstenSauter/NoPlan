using NoPlan.Api.Endpoints.V1.ToDos;
using NoPlan.Contracts.Responses.V1.ToDos;

namespace NoPlan.Api.Summaries.V1.ToDos;

public sealed class CreateEndpointSummary : Summary<CreateToDoEndpoint>
{
    public CreateEndpointSummary()
    {
        Summary = "Creates a new ToDo entity";
        Description = "Creates a new ToDo entity with the provided data and returns it";
        Response<ToDoResponse>(201, "Returns the successfully created ToDo entity");
        Response(400, "The request did not pass validation checks");
    }
}
