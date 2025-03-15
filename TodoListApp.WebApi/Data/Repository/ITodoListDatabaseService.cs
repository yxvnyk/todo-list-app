using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Data.Repository;

public interface ITodoListDatabaseService
{
    Task<IEnumerable<TodoListModel>> GetAllTodoList();

    Task AddTodoList(TodoListModel model);

    Task<bool> UpdateTodoList(TodoListModel model);

    Task<TodoListModel?> GetById(int id);

    Task<bool> DeleteByIdTodoList(int id);
}
