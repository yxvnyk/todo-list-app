using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ICommentWebApiService : ICrud<CommentDTO, CommentUpdateDTO>
{
    Task<IEnumerable<CommentDTO>?> GetAllByTaskAsync(int id);
}
