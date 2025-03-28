using TodoListApp.WebApi.Filters;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Data.Repository.Interfaces;

public interface ITagDatabaseService : ICrud<TagModel, TagFilter>
{
    Task<bool> TaskExist(int id);
}
