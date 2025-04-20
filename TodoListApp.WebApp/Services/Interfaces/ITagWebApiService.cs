using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ITagWebApiService : ICrud<TagDTO, TagUpdateDTO>
{
    Task<IEnumerable<TagDTO>?> GetAllByTaskAsync(int id);
}
