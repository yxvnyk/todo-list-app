using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Data.Repository.Interfaces;

/// <summary>
/// Defines the contract for a data access service handling comment-related operations.
/// Inherits basic CRUD functionality for <see cref="CommentDto"/> using <see cref="CommentUpdateDto"/> and <see cref="CommentFilter"/>.
/// </summary>
public interface ICommentDatabaseService : ICrud<CommentDto, CommentUpdateDto>
{
    /// <summary>
    /// Retrieves all entities based on the provided filter.
    /// </summary>
    /// <param name="filter">The filter to apply to the query.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains an enumerable list of entities.</returns>
    Task<IEnumerable<CommentDto>> GetAllAsync(CommentFilter filter);
}
