using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Data.Repository.Interfaces
{
    /// <summary>
    /// Defines the operations for managing tags in the system, including CRUD operations.
    /// </summary>
    public interface ITagDatabaseService : ICrud<TagDto, TagUpdateDto, TagFilter>
    {
        /// <summary>
        /// Retrieves all entities based on the provided filter.
        /// </summary>
        /// <param name="filter">The filter to apply to the query.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains an enumerable list of entities.</returns>
        Task<IEnumerable<TagDto>> GetAllAsync(TagFilter filter);
    }
}
