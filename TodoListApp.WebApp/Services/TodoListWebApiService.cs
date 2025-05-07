using System.Net;
using System.Text;
using System.Text.Json;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApp.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace TodoListApp.WebApp.Services;

internal class TodoListWebApiService : ITodoListWebApiService
{
    private readonly HttpClient httpClient;
    private readonly IHttpService httpService;

    public TodoListWebApiService(HttpClient httpClient, IHttpService httpService)
    {
        this.httpClient = httpClient;
        this.httpService = httpService;
    }

     public async Task<IEnumerable<TodoListDTO>?> GetAllAsync(string ownerId)
    {
        Console.WriteLine($"BaseAddress: {this.httpClient.BaseAddress}");
        var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/TodoList?OwnerId={ownerId}"));
        if (response == null)
        {
            return new List<TodoListDTO>();
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new List<TodoListDTO>();
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new List<TodoListDTO>();
        }

        Console.WriteLine($"BaseAddress: {this.httpClient.BaseAddress}");
        string json = await response.Content.ReadAsStringAsync();
        Console.WriteLine(json);
        List<TodoListDTO>? model = await response.Content.ReadFromJsonAsync<List<TodoListDTO>>();
        return model;
    }

    public async Task<HttpStatusCode?> AddAsync(TodoListDTO model)
    {
        ArgumentNullException.ThrowIfNull(model);
        using var todoItemJson = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            Application.Json);

        using var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/TodoList"), todoItemJson);

        if (response == null)
        {
            return null;
        }

        return response.StatusCode;
    }

    public async Task<HttpStatusCode?> UpdateAsync(TodoListUpdateDTO model, int id)
    {
        ArgumentNullException.ThrowIfNull(model);
        using var todoItemJson = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            Application.Json);

        using var response = await this.httpService.PutAsync(new Uri(this.httpClient.BaseAddress!, $"/api/TodoList/{id}"), todoItemJson);

        if (response == null)
        {
            return null;
        }

        return response.StatusCode;
    }

    public async Task<HttpStatusCode?> DeleteAsync(int id)
    {
        using var response = await this.httpService.DeleteAsync(new Uri(this.httpClient.BaseAddress!, $"/api/TodoList/{id}"));

        if (response == null)
        {
            return null;
        }

        return response.StatusCode;
    }

    public async Task<TodoListDTO?> GetByIdAsync(int id)
    {
        var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/TodoList/{id}"));
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
        TodoListDTO? model = await response.Content.ReadFromJsonAsync<TodoListDTO>();
        return model;
    }
}
