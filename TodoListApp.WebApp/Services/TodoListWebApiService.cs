using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using TodoListApp.WebApi.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TodoListApp.WebApp.Services;

internal class TodoListWebApiService : ITodoListWebApiService
{
    private readonly HttpClient httpClient;

    public TodoListWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<TodoListModel>?> GetAllTodoListsAsync()
    {
        var response = await this.httpClient.GetAsync(new Uri(this.httpClient.BaseAddress!, "/api/todos"));
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
        List<TodoListModel>? model = await response.Content.ReadFromJsonAsync<List<TodoListModel>>();
        return model;
    }

    public async Task<HttpStatusCode?> AddTodoListAsync(TodoListModel model)
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

    public async Task<HttpStatusCode?> UpdateTodoListAsync(TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        using var todoItemJson = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            Application.Json);

        using var response = await this.httpClient.PutAsync(new Uri(this.httpClient.BaseAddress!, "/api/todos"), todoItemJson);

        if (response == null)
        {
            return null;
        }

        return response.StatusCode;
    }

    public async Task<HttpStatusCode?> DeleteTodoListAsync(int id)
    {
        using var response = await this.httpClient.DeleteAsync(new Uri(this.httpClient.BaseAddress!, $"/api/todos/{id}"));

        if (response == null)
        {
            return null;
        }

        return response.StatusCode;
    }
}
