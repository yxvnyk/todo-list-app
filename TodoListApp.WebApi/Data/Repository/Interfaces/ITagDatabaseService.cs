using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Data.Repository.Interfaces;

public interface ITagDatabaseService : ICrud<TagModel>
{
    Task<bool> TaskExist(int id);
}
