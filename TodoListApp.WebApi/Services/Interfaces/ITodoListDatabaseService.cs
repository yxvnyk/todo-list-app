using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Data.Repository.Interfaces;

public interface ITodoListDatabaseService : ICrud<TodoListDTO, TodoListUpdateDTO, TodoListFilter>
{
    Task<bool> TodoListExist(int id);
}
