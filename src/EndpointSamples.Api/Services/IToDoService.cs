using EndpointSamples.Api.Contracts.Responses;

namespace EndpointSamples.Api.Services;

public interface IToDoService
{
    IEnumerable<ToDoResponse?> GetAll();
    ToDoResponse? Get(Guid id);
}