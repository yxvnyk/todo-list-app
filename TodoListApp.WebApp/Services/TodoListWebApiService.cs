using System.Net;
using System.Text;
using System.Text.Json;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApp.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace TodoListApp.WebApp.Services;

public class TodoListWebApiService : BaseApiService, ITodoListWebApiService
{
    public TodoListWebApiService(HttpClient httpClient, IHttpService httpService)
        : base(httpClient, httpService)
    {
    }

    public async Task<IEnumerable<TodoListDTO>?> GetAllAsync(string id)
    {
        Console.WriteLine($"BaseAddress: {this.httpClient.BaseAddress}");
        var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/TodoList?OwnerId={id}"));
        return await this.HandleResponseAsync<IEnumerable<TodoListDTO>?>(response);
    }

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

    public async Task<HttpStatusCode?> DeleteAsync(int id)
    {
        var response = await this.httpService.DeleteAsync(new Uri(this.httpClient.BaseAddress!, $"/api/TodoList/{id}"));
        return this.HandleResponseStatusAsync(response);
    }

    public async Task<TodoListDTO?> GetByIdAsync(int id)
    {
        var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/TodoList/{id}"));
        return await this.HandleResponseAsync<TodoListDTO?>(response);
    }
}
