using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Services;

public interface IToDoService
{
    Task<IEnumerable<ToDo>> GetAllAsync(Guid userId);
    Task<ToDo?> GetAsync(Guid id, Guid userId);
    Task<ToDo> CreateAsync(ToDo newToDo);
    Task<ToDo?> UpdateAsync(ToDo updatedToDo);
    Task<ToDo?> DeleteAsync(Guid id, Guid userId);
}
