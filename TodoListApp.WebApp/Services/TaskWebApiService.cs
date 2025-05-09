using System.Net;
using System.Text;
using System.Text.Json;
using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.PagingDTO;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApp.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace TodoListApp.WebApp.Services
{
    /// <summary>
    /// Service for interacting with the Task API, providing CRUD operations and advanced task queries.
    /// </summary>
    public class TaskWebApiService : BaseApiService, ITaskWebApiService
    {
        private readonly HttpClient httpClient;
        private readonly IHttpService httpService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskWebApiService"/> class.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used for making HTTP requests.</param>
        /// <param name="httpService">The service used to handle HTTP requests and responses.</param>
        public TaskWebApiService(HttpClient httpClient, IHttpService httpService)
            : base()
        {
            this.httpClient = httpClient;
            this.httpService = httpService;
        }

        /// <summary>
        /// Retrieves the owner ID of a task.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <returns>A task that represents the asynchronous operation, with the owner ID as the result.</returns>
        public async Task<string?> GetTaskOwnerId(int taskId)
        {
            var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task/GetOwnerId?taskId={taskId}"));
            var result = await this.HandleResponsePlainTextAsync(response);
            return result;
        }

        /// <summary>
        /// Retrieves tasks that belong to a specific to-do list.
        /// </summary>
        /// <param name="id">The ID of the to-do list.</param>
        /// <returns>A task that represents the asynchronous operation, with a <see cref="TaskPaging"/> object as the result.</returns>
        public async Task<TaskPaging?> GetAllByListAsync(int id)
        {
            TaskFilter filter = new TaskFilter()
            {
                TodoListId = id,
            };

            using var filterJson = new StringContent(
                JsonSerializer.Serialize(filter),
                Encoding.UTF8,
                Application.Json);

            var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Task/search"), filterJson);
            return await this.HandleResponseAsync<TaskPaging>(response);
        }

        /// <summary>
        /// Retrieves tasks based on a provided filter.
        /// </summary>
        /// <param name="filter">The filter to apply for task search.</param>
        /// <returns>A task that represents the asynchronous operation, with a <see cref="TaskPaging"/> object as the result.</returns>
        public async Task<TaskPaging?> GetAllByFilterAsync(TaskFilter filter)
        {
            using var filterJson = new StringContent(
                JsonSerializer.Serialize(filter),
                Encoding.UTF8,
                Application.Json);

            var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Task/search"), filterJson);
            return await this.HandleResponseAsync<TaskPaging>(response);
        }

        /// <summary>
        /// Retrieves tasks that are associated with a specific tag.
        /// </summary>
        /// <param name="tag">The tag to filter tasks by.</param>
        /// <returns>A task that represents the asynchronous operation, with a collection of <see cref="TaskPaging"/> objects as the result.</returns>
        public async Task<IEnumerable<TaskPaging>?> GetAllByTagAsync(string tag)
        {
            TaskFilter filter = new TaskFilter()
            {
                TagName = tag,
            };

            using var filterJson = new StringContent(
                JsonSerializer.Serialize(filter),
                Encoding.UTF8,
                Application.Json);

            var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Task/search"), filterJson);
            return await this.HandleResponseAsync<IEnumerable<TaskPaging>>(response);
        }

        /// <summary>
        /// Retrieves tasks that are assigned to a specific user.
        /// </summary>
        /// <param name="id">The ID of the assignee.</param>
        /// <returns>A task that represents the asynchronous operation, with a collection of <see cref="TaskPaging"/> objects as the result.</returns>
        public async Task<IEnumerable<TaskPaging>?> GetAllByAssigneeAsync(string id)
        {
            TaskFilter filter = new TaskFilter()
            {
                AssigneeId = id,
            };

            using var filterJson = new StringContent(
                JsonSerializer.Serialize(filter),
                Encoding.UTF8,
                Application.Json);

            var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Task/search"), filterJson);
            return await this.HandleResponseAsync<IEnumerable<TaskPaging>>(response);
        }

        /// <summary>
        /// Retrieves a task by its ID.
        /// </summary>
        /// <param name="id">The ID of the task.</param>
        /// <returns>A task that represents the asynchronous operation, with a <see cref="TaskDto"/> object as the result.</returns>
        public async Task<TaskDto?> GetByIdAsync(int id)
        {
            var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task/{id}"));
            return await this.HandleResponseAsync<TaskDto>(response);
        }

        /// <summary>
        /// Retrieves tasks assigned to a specific user.
        /// </summary>
        /// <param name="id">The ID of the assignee.</param>
        /// <returns>A task that represents the asynchronous operation, with a collection of <see cref="TaskDto"/> objects as the result.</returns>
        public async Task<IEnumerable<TaskDto>?> GetAllAsync(string id)
        {
            Console.WriteLine($"BaseAddress: {this.httpClient.BaseAddress}");
            var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task?AssigneeId={id}"));
            return await this.HandleResponseAsync<IEnumerable<TaskDto>>(response);
        }

        /// <summary>
        /// Adds a new task by sending a POST request to the Task API.
        /// </summary>
        /// <param name="model">The task to be added.</param>
        /// <returns>A task that represents the asynchronous operation, with the <see cref="HttpStatusCode"/> as the result.</returns>
        public async Task<HttpStatusCode?> AddAsync(TaskDto model)
        {
            using var todoItemJson = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                Application.Json);

            var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Task"), todoItemJson);
            return this.HandleResponseStatusAsync(response);
        }

        /// <summary>
        /// Updates an existing task by sending a PUT request to the Task API.
        /// </summary>
        /// <param name="model">The task update data.</param>
        /// <param name="id">The ID of the task to update.</param>
        /// <returns>A task that represents the asynchronous operation, with the <see cref="HttpStatusCode"/> as the result.</returns>
        public async Task<HttpStatusCode?> UpdateAsync(TaskUpdateDto model, int id)
        {
            using var todoItemJson = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                Application.Json);

            Console.WriteLine(await todoItemJson.ReadAsStringAsync());
            var response = await this.httpService.PutAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task/{id}"), todoItemJson);
            return this.HandleResponseStatusAsync(response);
        }

        /// <summary>
        /// Deletes a task by sending a DELETE request to the Task API.
        /// </summary>
        /// <param name="id">The ID of the task to delete.</param>
        /// <returns>A task that represents the asynchronous operation, with the <see cref="HttpStatusCode"/> as the result.</returns>
        public async Task<HttpStatusCode?> DeleteAsync(int id)
        {
            var response = await this.httpService.DeleteAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task/{id}"));
            return this.HandleResponseStatusAsync(response);
        }
    }
}
