using System.Net;
using System.Text;
using System.Text.Json;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApp.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace TodoListApp.WebApp.Services;

public class CommentWebApiService : BaseApiService, ICommentWebApiService
{
    public CommentWebApiService(HttpClient httpClient, IHttpService httpService)
        : base(httpClient, httpService)
    {
    }

    public async Task<HttpStatusCode?> AddAsync(CommentDTO model)
    {
        ArgumentNullException.ThrowIfNull(model);
        using var todoItemJson = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            Application.Json);

        var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Comment"), todoItemJson);
        return this.HandleResponseStatusAsync(response);
    }

    public async Task<HttpStatusCode?> DeleteAsync(int id)
    {
        var response = await this.httpService.DeleteAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Comment/{id}"));
        return this.HandleResponseStatusAsync(response);
    }

    public Task<IEnumerable<CommentDTO>?> GetAllAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CommentDTO>?> GetAllByTaskAsync(int id)
    {
        var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Comment?TaskId={id}"));
        return await this.HandleResponseAsync<IEnumerable<CommentDTO>>(response);
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
        var response = await this.httpService.PutAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Comment/{id}"), todoItemJson);
        return this.HandleResponseStatusAsync(response);
    }
}
