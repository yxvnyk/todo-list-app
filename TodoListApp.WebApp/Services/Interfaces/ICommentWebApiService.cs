using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.PagingDTO;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ICommentWebApiService : ICrud<CommentDTO, CommentUpdateDTO, CommentPaging>
{
    Task<IEnumerable<CommentDTO>?> GetAllByTaskAsync(int id);
}
