using TodoListApp.WebApi.Helpers.Filters;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Data.Repository.Interfaces;

public interface ITaskDatabaseService : ICrud<TaskModel, TaskFilter>
{
    Task<bool> TodoListExist(int id);
}
