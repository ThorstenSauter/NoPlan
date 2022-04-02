﻿using Microsoft.EntityFrameworkCore;
using NoPlan.Infrastructure.Data;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Services;

public class ToDoService : IToDoService
{
    private readonly PlannerContext _context;

    public ToDoService(PlannerContext context) =>
        _context = context;

    public async Task<IEnumerable<ToDo>> GetAllAsync(Guid userId) =>
        await _context.ToDos.AsNoTracking().WithPartitionKey(userId.ToString()).ToListAsync();

    public async Task<ToDo?> GetAsync(Guid id, Guid userId) =>
        await _context.ToDos
            .AsNoTracking()
            .WithPartitionKey(userId.ToString())
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<ToDo> CreateAsync(ToDo newToDo)
    {
        newToDo.Id = Guid.NewGuid();
        await _context.ToDos.AddAsync(newToDo);
        await _context.SaveChangesAsync();
        return newToDo;
    }

    public async Task<ToDo?> UpdateAsync(ToDo updatedToDo)
    {
        var toDo = await _context.ToDos.WithPartitionKey(updatedToDo.CreatedBy.ToString()).FirstOrDefaultAsync(t => t.Id == updatedToDo.Id);
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

    public async Task<ToDo?> DeleteAsync(Guid id, Guid userId)
    {
        var toDo = await _context.ToDos.WithPartitionKey(userId.ToString()).FirstOrDefaultAsync(t => t.Id == id);
        if (toDo is null)
        {
            return null;
        }

        _context.ToDos.Remove(toDo);
        await _context.SaveChangesAsync();
        return toDo;
    }
}
