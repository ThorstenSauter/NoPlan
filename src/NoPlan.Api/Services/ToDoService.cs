using Microsoft.EntityFrameworkCore;
using NoPlan.Infrastructure.Data;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Services;

/// <summary>
///     Implements the <see cref="IToDoService" /> using Entity Framework Cores <see cref="PlannerContext" />.
/// </summary>
public sealed class ToDoService : IToDoService
{
    private readonly PlannerContext _context;

    /// <summary>
    ///     Creates a new instance of <see cref="ToDoService" />.
    /// </summary>
    /// <param name="context">The <see cref="DbContext" /> to use for data access.</param>
    public ToDoService(PlannerContext context) =>
        _context = context;

    /// <inheritdoc />
    public async Task<IEnumerable<ToDo>> GetAllAsync(Guid userId) =>
        await _context.ToDos.Include(t => t.Tags).AsNoTracking().ToListAsync();

    /// <inheritdoc />
    public async Task<ToDo?> GetAsync(Guid id, Guid userId) =>
        await _context.ToDos.Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == id);

    /// <inheritdoc />
    public async Task<ToDo> CreateAsync(ToDo newToDo)
    {
        newToDo.Id = Guid.NewGuid();
        await _context.ToDos.AddAsync(newToDo);
        await _context.SaveChangesAsync();
        return newToDo;
    }

    /// <inheritdoc />
    public async Task<ToDo?> UpdateAsync(ToDo updatedToDo)
    {
        var toDo = await _context.ToDos.Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == updatedToDo.Id);
        if (toDo is null)
        {
            return null;
        }

        foreach (var tag in updatedToDo.Tags)
        {
            var existingToDo = toDo.Tags.FirstOrDefault(t => t == tag);
            if (existingToDo is not null)
            {
                tag.AssignedAt = existingToDo.AssignedAt;
            }
        }

        toDo.Title = updatedToDo.Title;
        toDo.Description = updatedToDo.Description;
        toDo.Tags = updatedToDo.Tags;
        await _context.SaveChangesAsync();
        return toDo;
    }

    /// <inheritdoc />
    public async Task<ToDo?> DeleteAsync(Guid id, Guid userId)
    {
        var toDo = await _context.ToDos.Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == id);
        if (toDo is null)
        {
            return null;
        }

        _context.ToDos.Remove(toDo);
        await _context.SaveChangesAsync();
        return toDo;
    }
}
