using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApp.Services.Interfaces
{
    /// <summary>
    /// Defines the operations for working with tags in the Web API.
    /// Inherits basic CRUD operations and extends with methods for managing tags by task and user.
    /// </summary>
    public interface ITagWebApiService : ICrud<TagDto, TagUpdateDto>
    {
        /// <summary>
        /// Retrieves all tags associated with a specific task.
        /// </summary>
        /// <param name="id">The task identifier.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of <see cref="TagDto"/>.</returns>
        Task<IEnumerable<TagDto>?> GetAllByTaskAsync(int id);

        /// <summary>
        /// Retrieves all tags associated with a specific owner and assignee.
        /// </summary>
        /// <param name="ownerId">The identifier of the task owner.</param>
        /// <param name="assigneeId">The identifier of the task assignee.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of <see cref="TagDto"/>.</returns>
        Task<IEnumerable<TagDto>?> GetAllAsync(string ownerId, string assigneeId);
    }
}
