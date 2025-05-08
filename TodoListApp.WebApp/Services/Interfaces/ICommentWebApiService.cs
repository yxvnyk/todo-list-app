using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.PagingDTO;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApp.Services.Interfaces
{
    /// <summary>
    /// Provides methods to interact with the comment API, including CRUD operations
    /// and fetching comments related to a specific task.
    /// </summary>
    public interface ICommentWebApiService : ICrud<CommentDTO, CommentUpdateDTO, CommentPaging>
    {
        /// <summary>
        /// Gets all comments associated with a specific task.
        /// </summary>
        /// <param name="id">The task identifier.</param>
        /// <returns>A collection of <see cref="CommentDTO"/> objects related to the task, or <c>null</c> if no comments are found.</returns>
        Task<IEnumerable<CommentDTO>?> GetAllByTaskAsync(int id);
    }
}
