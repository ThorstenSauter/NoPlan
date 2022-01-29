using EndpointsSamples.Infrastructure.Data;
using EndpointsSamples.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EndpointSamples.Api.Services;

public class ToDoService : IToDoService
{
    private readonly PlannerContext _context;

    public ToDoService(PlannerContext context) =>
        _context = context;

    public async Task<IEnumerable<ToDo>> GetAllAsync() =>
        await _context.ToDos.AsNoTracking().ToListAsync();

    public async Task<ToDo?> GetAsync(Guid id) =>
        await _context.ToDos.FindAsync(id);

    public async Task<ToDo> CreateAsync(ToDo newToDo)
    {
        newToDo.Id = Guid.NewGuid();
        await _context.ToDos.AddAsync(newToDo);
        await _context.SaveChangesAsync();
        return newToDo;
    }

    public async Task<ToDo?> UpdateAsync(ToDo updatedToDo)
    {
        var toDo = await _context.ToDos.FindAsync(updatedToDo.Id);
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
        var toDo = await _context.ToDos.FindAsync(id);
        if (toDo is null)
        {
            return null;
        }

        _context.ToDos.Remove(toDo);
        await _context.SaveChangesAsync();
        return toDo;
    }
}