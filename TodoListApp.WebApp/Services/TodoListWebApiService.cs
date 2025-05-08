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
    /// Service for interacting with the TodoList API, providing CRUD operations for todo lists.
    /// </summary>
    public class TodoListWebApiService : BaseApiService, ITodoListWebApiService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TodoListWebApiService"/> class.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to send HTTP requests.</param>
        /// <param name="httpService">The <see cref="IHttpService"/> used to handle HTTP requests.</param>
        public TodoListWebApiService(HttpClient httpClient, IHttpService httpService)
            : base(httpClient, httpService)
        {
        }

        /// <summary>
        /// Retrieves all todo lists owned by a specific user.
        /// </summary>
        /// <param name="id">The ID of the owner.</param>
        /// <returns>A task that represents the asynchronous operation, with a collection of <see cref="TodoListDTO"/> objects as the result.</returns>
        public async Task<IEnumerable<TodoListDTO>?> GetAllAsync(string id)
        {
            Console.WriteLine($"BaseAddress: {this.httpClient.BaseAddress}");
            var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/TodoList?OwnerId={id}"));
            return await this.HandleResponseAsync<IEnumerable<TodoListDTO>?>(response);
        }

        /// <summary>
        /// Adds a new todo list by sending a POST request to the TodoList API.
        /// </summary>
        /// <param name="model">The todo list to be added.</param>
        /// <returns>A task that represents the asynchronous operation, with the <see cref="HttpStatusCode"/> as the result.</returns>
        public async Task<HttpStatusCode?> AddAsync(TodoListDTO model)
        {
            ArgumentNullException.ThrowIfNull(model);
            using var todoItemJson = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                Application.Json);

            var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/TodoList"), todoItemJson);
            return this.HandleResponseStatusAsync(response);
        }

        /// <summary>
        /// Updates an existing todo list by sending a PUT request to the TodoList API.
        /// </summary>
        /// <param name="model">The todo list update data.</param>
        /// <param name="id">The ID of the todo list to update.</param>
        /// <returns>A task that represents the asynchronous operation, with the <see cref="HttpStatusCode"/> as the result.</returns>
        public async Task<HttpStatusCode?> UpdateAsync(TodoListUpdateDTO model, int id)
        {
            ArgumentNullException.ThrowIfNull(model);
            using var todoItemJson = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                Application.Json);

            var response = await this.httpService.PutAsync(new Uri(this.httpClient.BaseAddress!, $"/api/TodoList/{id}"), todoItemJson);
            return this.HandleResponseStatusAsync(response);
        }

        /// <summary>
        /// Deletes a todo list by sending a DELETE request to the TodoList API.
        /// </summary>
        /// <param name="id">The ID of the todo list to delete.</param>
        /// <returns>A task that represents the asynchronous operation, with the <see cref="HttpStatusCode"/> as the result.</returns>
        public async Task<HttpStatusCode?> DeleteAsync(int id)
        {
            var response = await this.httpService.DeleteAsync(new Uri(this.httpClient.BaseAddress!, $"/api/TodoList/{id}"));
            return this.HandleResponseStatusAsync(response);
        }

        /// <summary>
        /// Retrieves a todo list by its ID.
        /// </summary>
        /// <param name="id">The ID of the todo list.</param>
        /// <returns>A task that represents the asynchronous operation, with a <see cref="TodoListDTO"/> object as the result.</returns>
        public async Task<TodoListDTO?> GetByIdAsync(int id)
        {
            var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/TodoList/{id}"));
            return await this.HandleResponseAsync<TodoListDTO?>(response);
        }
    }
}
