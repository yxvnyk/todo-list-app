using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Data.Repository.Interfaces
{
    /// <summary>
    /// Defines the operations for managing tags in the system, including CRUD operations.
    /// </summary>
    public interface ITagDatabaseService : ICrud<TagDTO, TagUpdateDTO, TagFilter>
    {
    }
}
