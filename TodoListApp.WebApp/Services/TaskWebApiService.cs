using System.Net;
using System.Text;
using System.Text.Json;
using TodoListApp.WebApi.Helpers.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApp.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace TodoListApp.WebApp.Services;

public class TaskWebApiService : ITaskWebApiService
{
    private readonly HttpClient httpClient;

    public TaskWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<TaskDTO>?> GetAllByListAsync(int id)
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

        var response = await this.httpClient.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Task/search"), filterJson);
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

    public async Task<TaskDTO?> GetByIdAsync(int id)
    {
        var response = await this.httpClient.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task/{id}"));
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

    public async Task<IEnumerable<TaskDTO>?> GetAllAsync(int id)
    {
        Console.WriteLine($"BaseAddress: {this.httpClient.BaseAddress}");
        var response = await this.httpClient.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task?AssigneeId={id}"));
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

        using var response = await this.httpClient.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Task"), todoItemJson);

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
        using var response = await this.httpClient.PutAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task/{id}"), todoItemJson);

        if (response == null)
        {
            return null;
        }

        return response.StatusCode;
    }

    public async Task<HttpStatusCode?> DeleteAsync(int id)
    {
        using var response = await this.httpClient.DeleteAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Task/{id}"));

        if (response == null)
        {
            return null;
        }

        return response.StatusCode;
    }
}
