using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.PagingDTO;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApp.Services.Interfaces
{
    /// <summary>
    /// Defines the operations for managing to-do lists in the Web API.
    /// Inherits basic CRUD operations for handling to-do list data.
    /// </summary>
    public interface ITodoListWebApiService : ICrud<TodoListDTO, TodoListUpdateDTO, TodoListPaging>
    {
    }
}
