using TodoListApp.WebApi.Helpers.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Data.Repository.Interfaces;

public interface ICommentDatabaseService : ICrud<CommentDTO, CommentUpdateDTO, CommentFilter>
{
    Task<bool> TaskExist(int id);
}
