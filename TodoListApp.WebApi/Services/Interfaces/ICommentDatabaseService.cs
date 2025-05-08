using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Data.Repository.Interfaces;

/// <summary>
/// Defines the contract for a data access service handling comment-related operations.
/// Inherits basic CRUD functionality for <see cref="CommentDTO"/> using <see cref="CommentUpdateDTO"/> and <see cref="CommentFilter"/>.
/// </summary>
public interface ICommentDatabaseService : ICrud<CommentDTO, CommentUpdateDTO, CommentFilter>
{
}
