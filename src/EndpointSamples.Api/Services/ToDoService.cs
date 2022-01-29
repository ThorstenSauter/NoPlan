using EndpointSamples.Api.Contracts.Responses;

namespace EndpointSamples.Api.Services;

public class ToDoService : IToDoService
{
    private readonly HashSet<ToDoResponse> _data = new()
    {
        new()
        {
            Id = Guid.NewGuid(),
            Title = "Dentist",
            Description = "Go to the dentist"
        },
        new()
        {
            Id = Guid.NewGuid(),
            Title = "Take out the garbage",
            Description = "Take it out!"
        }
    };


    public IEnumerable<ToDoResponse?> GetAll() =>
        _data;

    public ToDoResponse? Get(Guid id) =>
        _data.FirstOrDefault(t => t.Id == id);
}