using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Entities;

namespace TodoListApp.DataAccess.Repositories.Interfaces;

public interface ITaskRepository : ICrud<TaskEntity, TaskFilter>
{
    new Task<(IQueryable<TaskEntity>, int)> GetAllAsync(TaskFilter filter);

    Task<bool> TaskExist(int id);
}
