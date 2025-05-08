using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.PagingDTO;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Data.Repository.Interfaces
{
    /// <summary>
    /// Defines operations for managing tasks in the system, including CRUD operations and additional task-specific functionality.
    /// </summary>
    public interface ITaskDatabaseService : ICrud<TaskDTO, TaskUpdateDTO, TaskFilter>
    {
        /// <summary>
        /// Checks if a task with the specified ID exists in the system.
        /// </summary>
        /// <param name="id">The ID of the task to check for existence.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating whether the task exists.</returns>
        Task<bool> TaskExist(int id);

        /// <summary>
        /// Retrieves a paginated list of tasks that match the specified filter.
        /// </summary>
        /// <param name="filter">The filter criteria to apply when retrieving tasks.</param>
        /// <returns>A task representing the asynchronous operation, containing a paginated list of tasks.</returns>
        new Task<TaskPaging> GetAllAsync(TaskFilter filter);

        /// <summary>
        /// Gets the owner ID of the specified task.
        /// </summary>
        /// <param name="taskId">The ID of the task to retrieve the owner ID for.</param>
        /// <returns>A string representing the owner ID of the task, or null if the task does not exist or does not have an owner.</returns>
        string? GetTaskOwnerId(int taskId);
    }
}
