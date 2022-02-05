using Microsoft.EntityFrameworkCore;
using NoPlan.Infrastructure.Data;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Services;

public class ToDoService : IToDoService
{
    private readonly PlannerContext _context;

    public ToDoService(PlannerContext context) =>
        _context = context;

    public async Task<IEnumerable<ToDo>> GetAllAsync() =>
        await _context.ToDos.AsNoTracking().ToListAsync();

    public async Task<ToDo?> GetAsync(Guid id) =>
        await _context.ToDos.WithPartitionKey(id.ToString()).AsNoTracking().FirstOrDefaultAsync();

    public async Task<ToDo> CreateAsync(ToDo newToDo)
    {
        newToDo.Id = Guid.NewGuid();
        await _context.ToDos.AddAsync(newToDo);
        await _context.SaveChangesAsync();
        return newToDo;
    }

    public async Task<ToDo?> UpdateAsync(ToDo updatedToDo)
    {
        var toDo = await _context.ToDos.WithPartitionKey(updatedToDo.Id.ToString()).FirstOrDefaultAsync();
        if (toDo is null)
        {
            return null;
        }

        toDo.Title = updatedToDo.Title;
        toDo.Description = updatedToDo.Description;
        await _context.SaveChangesAsync();
        return toDo;
    }

    public async Task<ToDo?> DeleteAsync(Guid id)
    {
        var toDo = await _context.ToDos.WithPartitionKey(id.ToString()).FirstOrDefaultAsync();
        if (toDo is null)
        {
            return null;
        }

        _context.ToDos.Remove(toDo);
        await _context.SaveChangesAsync();
        return toDo;
    }
}
