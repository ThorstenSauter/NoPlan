﻿using Microsoft.EntityFrameworkCore;
using NoPlan.Infrastructure.Data;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Features.ToDos;

/// <summary>
///     Implements the <see cref="IToDoService" /> using Entity Framework Cores <see cref="PlannerContext" />.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="ToDoService" /> class.
/// </remarks>
/// <param name="context">The <see cref="DbContext" /> to use for data access.</param>
public sealed class ToDoService(PlannerContext context) : IToDoService
{
    /// <inheritdoc />
    public async Task<IEnumerable<ToDo>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default) =>
        await context.ToDos
            .Include(t => t.Tags)
            .OrderByDescending(t => t.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<ToDo?> GetAsync(Guid id, Guid userId, CancellationToken cancellationToken = default) =>
        await context.ToDos
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    /// <inheritdoc />
    public async Task<ToDo> CreateAsync(ToDo newToDo)
    {
        ArgumentNullException.ThrowIfNull(newToDo);

        newToDo.Id = Guid.NewGuid();
        await context.ToDos.AddAsync(newToDo);
        await context.SaveChangesAsync();
        return newToDo;
    }

    /// <inheritdoc />
    public async Task<ToDo?> UpdateAsync(ToDo updatedToDo)
    {
        ArgumentNullException.ThrowIfNull(updatedToDo);

        var toDo = await context.ToDos
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == updatedToDo.Id);

        if (toDo is null)
        {
            return null;
        }

        foreach (var tag in updatedToDo.Tags)
        {
            var existingToDo = toDo.Tags.FirstOrDefault(t => t.Id == tag.Id);
            if (existingToDo is not null)
            {
                tag.AssignedAt = existingToDo.AssignedAt;
            }
        }

        toDo.Title = updatedToDo.Title;
        toDo.Description = updatedToDo.Description;
        toDo.Tags = updatedToDo.Tags;
        await context.SaveChangesAsync();
        return toDo;
    }

    /// <inheritdoc />
    public async Task<ToDo?> DeleteAsync(Guid id, Guid userId)
    {
        var toDo = await context.ToDos
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (toDo is null)
        {
            return null;
        }

        context.ToDos.Remove(toDo);
        await context.SaveChangesAsync();
        return toDo;
    }
}
