using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Entities;

namespace TodoListApp.DataAccess.Repositories.Interfaces;

public interface ITodoListRepository : ICrud<TodoListEntity, TodoListFilter>
{
    Task<bool> TodoListExist(int id);
}
