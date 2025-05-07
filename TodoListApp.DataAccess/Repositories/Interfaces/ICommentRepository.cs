using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Entities;

namespace TodoListApp.DataAccess.Repositories.Interfaces;

public interface ICommentRepository : ICrud<CommentEntity, CommentFilter>
{
}
