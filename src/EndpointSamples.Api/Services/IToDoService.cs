using EndpointsSamples.Infrastructure.Data.Models;

namespace EndpointSamples.Api.Services;

public interface IToDoService
{
    Task<IEnumerable<ToDo>> GetAllAsync();
    Task<ToDo?> GetAsync(Guid id);
    Task<ToDo> CreateAsync(ToDo newToDo);
    Task<ToDo?> UpdateAsync(ToDo updatedToDo);
    Task<ToDo?> DeleteAsync(Guid id);
}