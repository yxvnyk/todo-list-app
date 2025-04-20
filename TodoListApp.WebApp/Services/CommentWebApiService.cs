using System.Net;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Services;

public class CommentWebApiService : ICommentWebApiService
{
    private readonly HttpClient httpClient;

    public CommentWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public Task<HttpStatusCode?> AddAsync(CommentDTO model)
    {
        throw new NotImplementedException();
    }

    public Task<HttpStatusCode?> DeleteAsync(int id)
    {
        throw new NotImplementedException();
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

    public Task<TaskDTO?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<HttpStatusCode?> UpdateAsync(CommentUpdateDTO model, int id)
    {
        throw new NotImplementedException();
    }
}
