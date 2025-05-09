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
    /// Service for interacting with the To-doList API, providing CRUD operations for to-do lists.
    /// </summary>
    public class TodoListWebApiService : BaseApiService, ITodoListWebApiService
    {
        private readonly HttpClient httpClient;
        private readonly IHttpService httpService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TodoListWebApiService"/> class.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used for making HTTP requests.</param>
        /// <param name="httpService">The service used to handle HTTP requests and responses.</param>
        public TodoListWebApiService(HttpClient httpClient, IHttpService httpService)
            : base()
        {
            this.httpClient = httpClient;
            this.httpService = httpService;
        }

        /// <summary>
        /// Retrieves all to-do lists owned by a specific user.
        /// </summary>
        /// <param name="id">The ID of the owner.</param>
        /// <returns>A task that represents the asynchronous operation, with a collection of <see cref="TodoListDto"/> objects as the result.</returns>
        public async Task<IEnumerable<TodoListDto>?> GetAllAsync(string id)
        {
            Console.WriteLine($"BaseAddress: {this.httpClient.BaseAddress}");
            var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/TodoList?OwnerId={id}"));
            return await this.HandleResponseAsync<IEnumerable<TodoListDto>?>(response);
        }

        /// <summary>
        /// Adds a new to-do list by sending a POST request to the To-doList API.
        /// </summary>
        /// <param name="model">The to-do list to be added.</param>
        /// <returns>A task that represents the asynchronous operation, with the <see cref="HttpStatusCode"/> as the result.</returns>
        public async Task<HttpStatusCode?> AddAsync(TodoListDto model)
        {
            using var todoItemJson = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                Application.Json);

            var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/TodoList"), todoItemJson);
            return this.HandleResponseStatusAsync(response);
        }

        /// <summary>
        /// Updates an existing to-do list by sending a PUT request to the To-doList API.
        /// </summary>
        /// <param name="model">The to-do list update data.</param>
        /// <param name="id">The ID of the to-do list to update.</param>
        /// <returns>A task that represents the asynchronous operation, with the <see cref="HttpStatusCode"/> as the result.</returns>
        public async Task<HttpStatusCode?> UpdateAsync(TodoListUpdateDto model, int id)
        {
            using var todoItemJson = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                Application.Json);

            var response = await this.httpService.PutAsync(new Uri(this.httpClient.BaseAddress!, $"/api/TodoList/{id}"), todoItemJson);
            return this.HandleResponseStatusAsync(response);
        }

        /// <summary>
        /// Deletes a to-do list by sending a DELETE request to the To-doList API.
        /// </summary>
        /// <param name="id">The ID of the to-do list to delete.</param>
        /// <returns>A task that represents the asynchronous operation, with the <see cref="HttpStatusCode"/> as the result.</returns>
        public async Task<HttpStatusCode?> DeleteAsync(int id)
        {
            var response = await this.httpService.DeleteAsync(new Uri(this.httpClient.BaseAddress!, $"/api/TodoList/{id}"));
            return this.HandleResponseStatusAsync(response);
        }

        /// <summary>
        /// Retrieves a to-do list by its ID.
        /// </summary>
        /// <param name="id">The ID of the to-do list.</param>
        /// <returns>A task that represents the asynchronous operation, with a <see cref="TodoListDTO"/> object as the result.</returns>
        public async Task<TodoListDto?> GetByIdAsync(int id)
        {
            var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/TodoList/{id}"));
            return await this.HandleResponseAsync<TodoListDto?>(response);
        }
    }
}
