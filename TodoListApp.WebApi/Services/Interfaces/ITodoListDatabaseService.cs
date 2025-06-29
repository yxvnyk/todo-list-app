using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Data.Repository.Interfaces
{
    /// <summary>
    /// Defines operations for managing to-do lists in the system, including CRUD operations and additional to-do list-specific functionality.
    /// </summary>
    public interface ITodoListDatabaseService : ICrud<TodoListDto, TodoListUpdateDto>
    {
        /// <summary>
        /// Checks if a to-do list with the specified ID exists in the system.
        /// </summary>
        /// <param name="id">The ID of the to-do list to check for existence.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating whether the to-do list exists.</returns>
        Task<bool> TodoListExist(int id);

        /// <summary>
        /// Retrieves all entities based on the provided filter.
        /// </summary>
        /// <param name="filter">The filter to apply to the query.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains an enumerable list of entities.</returns>
        Task<IEnumerable<TodoListDto>> GetAllAsync(TodoListFilter filter);
    }
}
