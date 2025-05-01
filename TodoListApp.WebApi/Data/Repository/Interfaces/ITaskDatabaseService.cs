using TodoListApp.WebApi.Helpers.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.PagingDTO;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Data.Repository.Interfaces;

public interface ITaskDatabaseService : ICrud<TaskDTO, TaskUpdateDTO, TaskFilter>
{
    Task<bool> TodoListExist(int id);

    Task<TaskPaging> GetAllAsync(TaskFilter filter);
}
