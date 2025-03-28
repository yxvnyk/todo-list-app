using TodoListApp.WebApi.Filters;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Data.Repository.Interfaces;

public interface ITodoListDatabaseService : ICrud<TodoListModel, TodoListFilter>
{
}
