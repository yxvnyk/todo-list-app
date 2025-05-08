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
    /// Service for interacting with the Tag API, providing CRUD operations for tags.
    /// </summary>
    public class TagWebApiService : BaseApiService, ITagWebApiService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagWebApiService"/> class.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> used to send HTTP requests.</param>
        /// <param name="httpService">The <see cref="IHttpService"/> used to handle HTTP requests.</param>
        public TagWebApiService(HttpClient client, IHttpService httpService)
            : base(client, httpService)
        {
        }

        /// <summary>
        /// Adds a new tag by sending a POST request to the Tag API.
        /// </summary>
        /// <param name="model">The tag to be added.</param>
        /// <returns>A task that represents the asynchronous operation, with the <see cref="HttpStatusCode"/> as the result.</returns>
        public async Task<HttpStatusCode?> AddAsync(TagDTO model)
        {
            ArgumentNullException.ThrowIfNull(model);
            using var todoItemJson = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                Application.Json);

            var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Tag"), todoItemJson);
            return this.HandleResponseStatusAsync(response);
        }

        /// <summary>
        /// Deletes a tag by sending a DELETE request to the Tag API.
        /// </summary>
        /// <param name="id">The ID of the tag to delete.</param>
        /// <returns>A task that represents the asynchronous operation, with the <see cref="HttpStatusCode"/> as the result.</returns>
        public async Task<HttpStatusCode?> DeleteAsync(int id)
        {
            var response = await this.httpService.DeleteAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Tag/{id}"));
            return this.HandleResponseStatusAsync(response);
        }

        /// <summary>
        /// Retrieves a list of tags by owner ID and assignee ID.
        /// </summary>
        /// <param name="ownerId">The ID of the tag owner.</param>
        /// <param name="assigneeId">The ID of the assignee.</param>
        /// <returns>A task that represents the asynchronous operation, with a collection of <see cref="TagDTO"/> as the result.</returns>
        public async Task<IEnumerable<TagDTO>?> GetAllAsync(string ownerId, string assigneeId)
        {
            Console.WriteLine($"BaseAddress: {this.httpClient.BaseAddress}");
            var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Tag?OwnerId={ownerId}&AssigneeId={assigneeId}"));
            return await this.HandleResponseAsync<IEnumerable<TagDTO>>(response);
        }

        /// <summary>
        /// Retrieves a list of tags associated with a specific task.
        /// </summary>
        /// <param name="id">The ID of the task.</param>
        /// <returns>A task that represents the asynchronous operation, with a collection of <see cref="TagDTO"/> as the result.</returns>
        public async Task<IEnumerable<TagDTO>?> GetAllByTaskAsync(int id)
        {
            var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Tag?TaskId={id}"));
            return await this.HandleResponseAsync<IEnumerable<TagDTO>>(response);
        }

        /// <summary>
        /// Updates an existing tag by sending a PUT request to the Tag API.
        /// </summary>
        /// <param name="model">The tag update data.</param>
        /// <param name="id">The ID of the tag to update.</param>
        /// <returns>A task that represents the asynchronous operation, with the <see cref="HttpStatusCode"/> as the result.</returns>
        public async Task<HttpStatusCode?> UpdateAsync(TagUpdateDTO model, int id)
        {
            ArgumentNullException.ThrowIfNull(model);
            using var todoItemJson = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                Application.Json);

            Console.WriteLine(await todoItemJson.ReadAsStringAsync());
            var response = await this.httpService.PutAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Tag/{id}"), todoItemJson);
            response = await this.HandleResponseAsync<HttpResponseMessage>(response);

            return response?.StatusCode;
        }

        /// <summary>
        /// Not implemented: Retrieves a tag by its ID.
        /// </summary>
        /// <param name="id">The ID of the tag.</param>
        /// <returns>A task that represents the asynchronous operation, with the <see cref="TagDTO"/> as the result.</returns>
        public Task<TagDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented: Retrieves a list of tags by ID.
        /// </summary>
        /// <param name="id">The ID of the tags.</param>
        /// <returns>A task that represents the asynchronous operation, with a collection of <see cref="TagDTO"/> as the result.</returns>
        public Task<IEnumerable<TagDTO>?> GetAllAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
