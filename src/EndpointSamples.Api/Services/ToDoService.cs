using EndpointSamples.Api.Models;

namespace EndpointSamples.Api.Services;

public class ToDoService : IToDoService
{
    private readonly HashSet<ToDo> _data = new()
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


    public IEnumerable<ToDo> GetAll() =>
        _data;

    public ToDo? Get(Guid id) =>
        _data.FirstOrDefault(t => t.Id == id);

    public ToDo Create(ToDo newToDo)
    {
        newToDo.Id = Guid.NewGuid();
        _data.Add(newToDo);
        return newToDo;
    }

    public ToDo? Update(ToDo updatedToDo)
    {
        var toDo = _data.FirstOrDefault(t => t.Id == updatedToDo.Id);
        if (toDo is null)
        {
            return null;
        }

        toDo.Title = updatedToDo.Title;
        toDo.Description = updatedToDo.Description;
        return toDo;
    }

    public ToDo? Delete(Guid id)
    {
        var toDo = _data.FirstOrDefault(t => t.Id == id);
        if (toDo is null)
        {
            return null;
        }

        _data.Remove(toDo);
        return toDo;
    }
}