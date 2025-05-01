using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.PagingDTO;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ITagWebApiService : ICrud<TagDTO, TagUpdateDTO, TagPaging>
{
    Task<IEnumerable<TagDTO>?> GetAllByTaskAsync(int id);
}
