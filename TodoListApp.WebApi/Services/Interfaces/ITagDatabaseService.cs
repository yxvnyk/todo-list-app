using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Data.Repository.Interfaces;

public interface ITagDatabaseService : ICrud<TagDTO, TagUpdateDTO, TagFilter>
{
    Task<bool> TaskExist(int id);
}
