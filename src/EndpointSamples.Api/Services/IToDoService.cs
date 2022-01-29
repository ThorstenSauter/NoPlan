using EndpointSamples.Api.Models;

namespace EndpointSamples.Api.Services;

public interface IToDoService
{
    IEnumerable<ToDo> GetAll();
    ToDo? Get(Guid id);
    ToDo Create(ToDo newToDo);
    ToDo? Update(ToDo updatedToDo);
    ToDo? Delete(Guid id);
}