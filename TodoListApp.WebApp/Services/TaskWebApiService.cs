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

public class TaskWebApiService : ITaskWebApiService
{
    private readonly HttpClient httpClient;

    private readonly IHttpService httpService;

    public TaskWebApiService(HttpClient httpClient, IHttpService httpService)
    {
        this.httpClient = httpClient;
        this.httpService = httpService;
    }

    public async Task<string> GetTaskOwnerId(int taskId)
    {
        var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task/GetOwnerId?taskId={taskId}"));
        if (response == null)
        {
            return string.Empty;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return string.Empty;
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return string.Empty;
        }

        Console.WriteLine($"BaseAddress: {this.httpClient.BaseAddress}");
        string json = await response.Content.ReadAsStringAsync();
        Console.WriteLine(json);
        string? ownerId = await response.Content.ReadAsStringAsync();
        return ownerId;
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
        if (response == null)
        {
            return null;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        string json = await response.Content.ReadAsStringAsync();
        Console.WriteLine(json);
        TaskPaging? model = await response.Content.ReadFromJsonAsync<TaskPaging>();
        return model;
    }

    public async Task<TaskPaging?> GetAllByFilterAsync(TaskFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);
        using var filterJson = new StringContent(
            JsonSerializer.Serialize(filter),
            Encoding.UTF8,
            Application.Json);

        var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Task/search"), filterJson);
        if (response == null)
        {
            return null;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        string json = await response.Content.ReadAsStringAsync();
        Console.WriteLine(json);
        TaskPaging? model = await response.Content.ReadFromJsonAsync<TaskPaging>();
        return model;
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
        if (response == null)
        {
            return null;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        string json = await response.Content.ReadAsStringAsync();
        Console.WriteLine(json);
        List<TaskPaging>? model = await response.Content.ReadFromJsonAsync<List<TaskPaging>>();
        return model;
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
        if (response == null)
        {
            return null;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        string json = await response.Content.ReadAsStringAsync();
        Console.WriteLine(json);
        List<TaskPaging>? model = await response.Content.ReadFromJsonAsync<List<TaskPaging>>();
        return model;
    }

    public async Task<TaskDTO?> GetByIdAsync(int id)
    {
        var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task/{id}"));
        if (response == null)
        {
            return null;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        string json = await response.Content.ReadAsStringAsync();
        Console.WriteLine(json);
        TaskDTO? model = await response.Content.ReadFromJsonAsync<TaskDTO>();
        return model;
    }

    public async Task<IEnumerable<TaskDTO>?> GetAllAsync(string id)
    {
        Console.WriteLine($"BaseAddress: {this.httpClient.BaseAddress}");
        var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task?AssigneeId={id}"));
        if (response == null)
        {
            return null;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        string json = await response.Content.ReadAsStringAsync();
        Console.WriteLine(json);
        List<TaskDTO>? model = await response.Content.ReadFromJsonAsync<List<TaskDTO>>();
        return model;
    }

    public async Task<HttpStatusCode?> AddAsync(TaskDTO model)
    {
        ArgumentNullException.ThrowIfNull(model);
        using var todoItemJson = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            Application.Json);

        using var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Task"), todoItemJson);

        if (response == null)
        {
            return null;
        }

        return response.StatusCode;
    }

    public async Task<HttpStatusCode?> UpdateAsync(TaskUpdateDTO model, int id)
    {
        ArgumentNullException.ThrowIfNull(model);
        using var todoItemJson = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            Application.Json);

        Console.WriteLine(await todoItemJson.ReadAsStringAsync());
        using var response = await this.httpService.PutAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task/{id}"), todoItemJson);

        if (response == null)
        {
            return null;
        }

        return response.StatusCode;
    }

    public async Task<HttpStatusCode?> DeleteAsync(int id)
    {
        using var response = await this.httpService.DeleteAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task/{id}"));

        if (response == null)
        {
            return null;
        }

        return response.StatusCode;
    }
}
