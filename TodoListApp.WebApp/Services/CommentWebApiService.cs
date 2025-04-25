using System.Net;
using System.Text;
using System.Text.Json;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApp.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace TodoListApp.WebApp.Services;

public class CommentWebApiService : ICommentWebApiService
{
    private readonly HttpClient httpClient;

    public CommentWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<HttpStatusCode?> AddAsync(CommentDTO model)
    {
        ArgumentNullException.ThrowIfNull(model);
        using var todoItemJson = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            Application.Json);

        using var response = await this.httpClient.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Comment"), todoItemJson);

        if (response == null)
        {
            return null;
        }

        return response.StatusCode;
    }

    public async Task<HttpStatusCode?> DeleteAsync(int id)
    {
        using var response = await this.httpClient.DeleteAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Comment/{id}"));

        if (response == null)
        {
            return null;
        }

        return response.StatusCode;
    }

    public Task<IEnumerable<CommentDTO>?> GetAllAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CommentDTO>?> GetAllByTaskAsync(int id)
    {
        var response = await this.httpClient.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Comment?TaskId={id}"));
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
        List<CommentDTO>? model = await response.Content.ReadFromJsonAsync<List<CommentDTO>>();
        return model;
    }

    public Task<CommentDTO?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<HttpStatusCode?> UpdateAsync(CommentUpdateDTO model, int id)
    {
        ArgumentNullException.ThrowIfNull(model);
        using var todoItemJson = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            Application.Json);

        Console.WriteLine(await todoItemJson.ReadAsStringAsync());
        using var response = await this.httpClient.PutAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Comment/{id}"), todoItemJson);

        if (response == null)
        {
            return null;
        }

        return response.StatusCode;
    }
}
