using System.Net;
using System.Net.Http;
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

    public TodoListWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<TodoListDTO>?> GetAllAsync(int id)
    {
        Console.WriteLine($"BaseAddress: {this.httpClient.BaseAddress}");
        var response = await this.httpClient.GetAsync(new Uri(this.httpClient.BaseAddress!, "/api/TodoList"));
        if (response == null)
        {
            return null;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
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

        using var response = await this.httpClient.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/todos"), todoItemJson);

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

        using var response = await this.httpClient.PutAsync(new Uri(this.httpClient.BaseAddress!, $"/api/todos/{id}"), todoItemJson);

        if (response == null)
        {
            return null;
        }

        return response.StatusCode;
    }

    public async Task<HttpStatusCode?> DeleteAsync(int id)
    {
        using var response = await this.httpClient.DeleteAsync(new Uri(this.httpClient.BaseAddress!, $"/api/todos/{id}"));

        if (response == null)
        {
            return null;
        }

        return response.StatusCode;
    }

    public Task<TaskDTO?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
