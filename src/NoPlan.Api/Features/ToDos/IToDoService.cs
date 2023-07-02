using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Api.Features.ToDos;

/// <summary>
///     Provides CRUD functionality for the <see cref="ToDo" /> type.
/// </summary>
public interface IToDoService
{
    /// <summary>
    ///     Retrieves all <see cref="ToDo" />s for the given user identifier.
    /// </summary>
    /// <param name="userId">The identifier of the user for which to retrieve the <see cref="ToDo" />s.</param>
    /// <param name="cancellationToken">Allows for early cancellation of the retrieval.</param>
    /// <returns>The users <see cref="ToDo" />s.</returns>
    Task<IEnumerable<ToDo>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves the <see cref="ToDo" /> for the given identifier and user identifier.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="ToDo" /> to retrieve.</param>
    /// <param name="userId">The identifier of the user for which to retrieve the <see cref="ToDo" />.</param>
    /// <param name="cancellationToken">Allows for early cancellation of the retrieval.</param>
    /// <returns>The specified <see cref="ToDo" />.</returns>
    Task<ToDo?> GetAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates a new <see cref="ToDo" />.
    /// </summary>
    /// <param name="newToDo">The <see cref="ToDo" /> to create.</param>
    /// <returns>The created <see cref="ToDo" />.</returns>
    Task<ToDo> CreateAsync(ToDo newToDo);

    /// <summary>
    ///     Updates the specified <see cref="ToDo" />.
    /// </summary>
    /// <param name="updatedToDo">The <see cref="ToDo" /> containing the updated values.</param>
    /// <returns>The updated <see cref="ToDo" />.</returns>
    Task<ToDo?> UpdateAsync(ToDo updatedToDo);

    /// <summary>
    ///     Deletes the <see cref="ToDo" /> with the given identifier and user identifier.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="ToDo" /> to delete.</param>
    /// <param name="userId">The identifier of the user for which to delete the <see cref="ToDo" />.</param>
    /// <returns>The deleted <see cref="ToDo" />.</returns>
    Task<ToDo?> DeleteAsync(Guid id, Guid userId);
}
