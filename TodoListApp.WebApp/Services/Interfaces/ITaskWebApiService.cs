using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ITaskWebApiService : ICrud<TaskDTO, TaskUpdateDTO>
{
    Task<IEnumerable<TaskDTO>?> GetAllByListAsync(int id);

    Task<IEnumerable<TaskDTO>?> GetAllByTagAsync(string tag);

}
