using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Data.Repository.Interfaces;

public interface ITaskDatabaseService : ICrud<TaskModel>
{
    Task<bool> TodoListExist(int id);
}
