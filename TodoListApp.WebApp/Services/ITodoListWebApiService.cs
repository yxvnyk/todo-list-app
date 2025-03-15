using System.Net;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Services;

public interface ITodoListWebApiService
{
    Task<IEnumerable<TodoListModel>?> GetAllTodoListsAsync();

    Task<HttpStatusCode?> DeleteTodoListAsync(int id);

    Task<HttpStatusCode?> AddTodoListAsync(TodoListModel model);

    Task<HttpStatusCode?> UpdateTodoListAsync(TodoListModel model);
}
