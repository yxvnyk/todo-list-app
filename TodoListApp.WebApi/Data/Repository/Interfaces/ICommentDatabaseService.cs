using TodoListApp.WebApi.Filters;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Data.Repository.Interfaces;

public interface ICommentDatabaseService : ICrud<CommentModel, CommentFilter>
{
    Task<bool> TaskExist(int id);
}
