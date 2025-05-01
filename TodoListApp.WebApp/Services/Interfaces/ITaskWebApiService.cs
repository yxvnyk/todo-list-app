using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Helpers.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.PagingDTO;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ITaskWebApiService : ICrud<TaskDTO, TaskUpdateDTO, TaskPaging>
{
    Task<TaskPaging?> GetAllByListAsync(int id);

    Task<IEnumerable<TaskPaging>?> GetAllByTagAsync(string tag);

    Task<IEnumerable<TaskPaging>?> GetAllByAssigneeAsync(string id);

    Task<TaskPaging?> GetAllByFilterAsync(TaskFilter filter);
}
