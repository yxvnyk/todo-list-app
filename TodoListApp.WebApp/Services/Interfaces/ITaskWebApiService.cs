using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.PagingDTO;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApp.Services.Interfaces
{
    /// <summary>
    /// Defines the operations for working with tasks in the Web API.
    /// Inherits basic CRUD operations and extends with methods for managing tasks by various criteria.
    /// </summary>
    public interface ITaskWebApiService : ICrud<TaskDto, TaskUpdateDto>
    {
        /// <summary>
        /// Retrieves all tasks associated with a specific task list.
        /// </summary>
        /// <param name="id">The identifier of the task list.</param>
        /// <returns>A task representing the asynchronous operation, containing a <see cref="TaskPaging"/> object for the tasks.</returns>
        Task<TaskPaging?> GetAllByListAsync(int id);

        /// <summary>
        /// Retrieves all tasks associated with a specific tag.
        /// </summary>
        /// <param name="tag">The tag name.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of <see cref="TaskPaging"/> objects for the tasks.</returns>
        Task<IEnumerable<TaskPaging>?> GetAllByTagAsync(string tag);

        /// <summary>
        /// Retrieves all tasks assigned to a specific assignee.
        /// </summary>
        /// <param name="id">The identifier of the assignee.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of <see cref="TaskPaging"/> objects for the tasks.</returns>
        Task<IEnumerable<TaskPaging>?> GetAllByAssigneeAsync(string id);

        /// <summary>
        /// Retrieves all tasks that match a specific filter.
        /// </summary>
        /// <param name="filter">The filter criteria for the tasks.</param>
        /// <returns>A task representing the asynchronous operation, containing a <see cref="TaskPaging"/> object for the filtered tasks.</returns>
        Task<TaskPaging?> GetAllByFilterAsync(TaskFilter filter);

        /// <summary>
        /// Retrieves the owner ID of a task.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns>A task representing the asynchronous operation, containing the owner ID of the task.</returns>
        Task<string?> GetTaskOwnerId(int taskId);
    }
}
