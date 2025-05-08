using System.Net;
using System.Text;
using System.Text.Json;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApp.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace TodoListApp.WebApp.Services
{
    /// <summary>
    /// Service for interacting with the comment-related API endpoints.
    /// </summary>
    public class CommentWebApiService : BaseApiService, ICommentWebApiService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentWebApiService"/> class.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used for making HTTP requests.</param>
        /// <param name="httpService">The service used to handle HTTP requests and responses.</param>
        public CommentWebApiService(HttpClient httpClient, IHttpService httpService)
            : base(httpClient, httpService)
        {
        }

        /// <summary>
        /// Adds a new comment to the API.
        /// </summary>
        /// <param name="model">The <see cref="CommentDTO"/> object to be added.</param>
        /// <returns>A task that represents the asynchronous operation, with the HTTP status code as the result.</returns>
        public async Task<HttpStatusCode?> AddAsync(CommentDTO model)
        {
            ArgumentNullException.ThrowIfNull(model);
            using var todoItemJson = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                Application.Json);

            var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Comment"), todoItemJson);
            return this.HandleResponseStatusAsync(response);
        }

        /// <summary>
        /// Deletes a comment by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the comment to delete.</param>
        /// <returns>A task that represents the asynchronous operation, with the HTTP status code as the result.</returns>
        public async Task<HttpStatusCode?> DeleteAsync(int id)
        {
            var response = await this.httpService.DeleteAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Comment/{id}"));
            return this.HandleResponseStatusAsync(response);
        }

        /// <summary>
        /// Retrieves all comments associated with a specific task.
        /// </summary>
        /// <param name="id">The identifier of the task to retrieve comments for.</param>
        /// <returns>A task that represents the asynchronous operation, with a collection of <see cref="CommentDTO"/> objects as the result.</returns>
        public async Task<IEnumerable<CommentDTO>?> GetAllByTaskAsync(int id)
        {
            var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Comment?TaskId={id}"));
            return await this.HandleResponseAsync<IEnumerable<CommentDTO>>(response);
        }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <param name="model">The <see cref="CommentUpdateDTO"/> object containing the updated comment data.</param>
        /// <param name="id">The identifier of the comment to update.</param>
        /// <returns>A task that represents the asynchronous operation, with the HTTP status code as the result.</returns>
        public async Task<HttpStatusCode?> UpdateAsync(CommentUpdateDTO model, int id)
        {
            ArgumentNullException.ThrowIfNull(model);
            using var todoItemJson = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                Application.Json);

            Console.WriteLine(await todoItemJson.ReadAsStringAsync());
            var response = await this.httpService.PutAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Comment/{id}"), todoItemJson);
            return this.HandleResponseStatusAsync(response);
        }

        // Not implemented methods:
        public Task<IEnumerable<CommentDTO>?> GetAllAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<CommentDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
