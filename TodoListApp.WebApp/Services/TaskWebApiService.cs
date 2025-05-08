using System.Net;
using System.Text;
using System.Text.Json;
using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.PagingDTO;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApp.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace TodoListApp.WebApp.Services;

public class TaskWebApiService : BaseApiService, ITaskWebApiService
{
    public TaskWebApiService(HttpClient httpClient, IHttpService httpService)
        : base(httpClient, httpService)
    {
    }

    public async Task<string?> GetTaskOwnerId(int taskId)
    {
        var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task/GetOwnerId?taskId={taskId}"));
        var result = await this.HandleResponsePlainTextAsync(response);
        return result;
    }

    public async Task<TaskPaging?> GetAllByListAsync(int id)
    {
        TaskFilter filter = new TaskFilter()
        {
            TodoListId = id,
        };

        ArgumentNullException.ThrowIfNull(filter);
        using var filterJson = new StringContent(
            JsonSerializer.Serialize(filter),
            Encoding.UTF8,
            Application.Json);

        var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Task/search"), filterJson);
        return await this.HandleResponseAsync<TaskPaging>(response);
    }

    public async Task<TaskPaging?> GetAllByFilterAsync(TaskFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);
        using var filterJson = new StringContent(
            JsonSerializer.Serialize(filter),
            Encoding.UTF8,
            Application.Json);

        var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Task/search"), filterJson);
        return await this.HandleResponseAsync<TaskPaging>(response);
    }

    public async Task<IEnumerable<TaskPaging>?> GetAllByTagAsync(string tag)
    {
        TaskFilter filter = new TaskFilter()
        {
            TagName = tag,
        };

        ArgumentNullException.ThrowIfNull(filter);
        using var filterJson = new StringContent(
            JsonSerializer.Serialize(filter),
            Encoding.UTF8,
            Application.Json);

        var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Task/search"), filterJson);
        return await this.HandleResponseAsync<IEnumerable<TaskPaging>>(response);
    }

    public async Task<IEnumerable<TaskPaging>?> GetAllByAssigneeAsync(string id)
    {
        TaskFilter filter = new TaskFilter()
        {
            AssigneeId = id,
        };

        ArgumentNullException.ThrowIfNull(filter);
        using var filterJson = new StringContent(
            JsonSerializer.Serialize(filter),
            Encoding.UTF8,
            Application.Json);

        var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Task/search"), filterJson);
        return await this.HandleResponseAsync<IEnumerable<TaskPaging>>(response);
    }

    public async Task<TaskDTO?> GetByIdAsync(int id)
    {
        var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task/{id}"));
        return await this.HandleResponseAsync<TaskDTO>(response);
    }

    public async Task<IEnumerable<TaskDTO>?> GetAllAsync(string id)
    {
        Console.WriteLine($"BaseAddress: {this.httpClient.BaseAddress}");
        var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task?AssigneeId={id}"));
        return await this.HandleResponseAsync<IEnumerable<TaskDTO>>(response);

    }

    public async Task<HttpStatusCode?> AddAsync(TaskDTO model)
    {
        ArgumentNullException.ThrowIfNull(model);
        using var todoItemJson = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            Application.Json);

        var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Task"), todoItemJson);
        return this.HandleResponseStatusAsync(response);
    }

    public async Task<HttpStatusCode?> UpdateAsync(TaskUpdateDTO model, int id)
    {
        ArgumentNullException.ThrowIfNull(model);
        using var todoItemJson = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            Application.Json);

        Console.WriteLine(await todoItemJson.ReadAsStringAsync());
        var response = await this.httpService.PutAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task/{id}"), todoItemJson);
        return this.HandleResponseStatusAsync(response);
    }

    public async Task<HttpStatusCode?> DeleteAsync(int id)
    {
        var response = await this.httpService.DeleteAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task/{id}"));
        return this.HandleResponseStatusAsync(response);
    }
}
