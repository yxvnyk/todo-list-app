using System.Net;
using System.Text;
using System.Text.Json;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApp.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace TodoListApp.WebApp.Services;

public class TagWebApiService : BaseApiService, ITagWebApiService
{
    public TagWebApiService(HttpClient client, IHttpService httpService)
        : base(client, httpService)
    {
    }

    public async Task<HttpStatusCode?> AddAsync(TagDTO model)
    {
        ArgumentNullException.ThrowIfNull(model);
        using var todoItemJson = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            Application.Json);

        var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Tag"), todoItemJson);
        return this.HandleResponseStatusAsync(response);
    }

    public async Task<HttpStatusCode?> DeleteAsync(int id)
    {
        var response = await this.httpService.DeleteAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Tag/{id}"));
        return this.HandleResponseStatusAsync(response);
    }

    public async Task<IEnumerable<TagDTO>?> GetAllAsync(string ownerId, string assigneeId)
    {
        Console.WriteLine($"BaseAddress: {this.httpClient.BaseAddress}");
        var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Tag?OwnerId={ownerId}&AssigneeId={assigneeId}"));
        return await this.HandleResponseAsync<IEnumerable<TagDTO>>(response);
    }

    public async Task<IEnumerable<TagDTO>?> GetAllByTaskAsync(int id)
    {
        var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Tag?TaskId={id}"));
        return await this.HandleResponseAsync<IEnumerable<TagDTO>>(response);
    }

    public async Task<HttpStatusCode?> UpdateAsync(TagUpdateDTO model, int id)
    {
        ArgumentNullException.ThrowIfNull(model);
        using var todoItemJson = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            Application.Json);

        Console.WriteLine(await todoItemJson.ReadAsStringAsync());
        var response = await this.httpService.PutAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Tag/{id}"), todoItemJson);
        response = await this.HandleResponseAsync<HttpResponseMessage>(response);

        return response?.StatusCode;
    }

    public Task<TagDTO?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TagDTO>?> GetAllAsync(string id)
    {
        throw new NotImplementedException();
    }
}
