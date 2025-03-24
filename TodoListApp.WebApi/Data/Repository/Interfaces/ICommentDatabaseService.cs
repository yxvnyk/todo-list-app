using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Data.Repository.Interfaces;

public interface ICommentDatabaseService : ICrud<CommentModel>
{
    Task<bool> TaskExist(int id);
}
