using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.PagingDTO;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Data.Repository.Interfaces
{
    /// <summary>
    /// Defines operations for managing tasks in the system, including CRUD operations and additional task-specific functionality.
    /// </summary>
    public interface ITaskDatabaseService : ICrud<TaskDto, TaskUpdateDto>
    {
        /// <summary>
        /// Retrieves all entities based on the provided filter.
        /// </summary>
        /// <param name="filter">The filter to apply to the query.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains an enumerable list of entities.</returns>
        Task<TaskPaging> GetAllAsync(TaskFilter filter);

        /// <summary>
        /// Checks if a task with the specified ID exists in the system.
        /// </summary>
        /// <param name="id">The ID of the task to check for existence.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating whether the task exists.</returns>
        Task<bool> TaskExist(int id);

        /// <summary>
        /// Gets the owner ID of the specified task.
        /// </summary>
        /// <param name="taskId">The ID of the task to retrieve the owner ID for.</param>
        /// <returns>A string representing the owner ID of the task, or null if the task does not exist or does not have an owner.</returns>
        string? GetTaskOwnerId(int taskId);
    }
}
