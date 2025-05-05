using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using NuGet.Common;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApp.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace TodoListApp.WebApp.Services;

public class TagWebApiService : ITagWebApiService
{
    private readonly HttpClient httpClient;
    private readonly IHttpService httpService;

    public TagWebApiService(HttpClient client, IHttpService httpService)
    {
        this.httpClient = client;
        this.httpService = httpService;
    }

    public async Task<HttpStatusCode?> AddAsync(TagDTO model)
    {
        ArgumentNullException.ThrowIfNull(model);
        using var todoItemJson = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            Application.Json);

        using var response = await this.httpService.PostAsync(new Uri(this.httpClient.BaseAddress!, "/api/Tag"), todoItemJson);

        if (response == null)
        {
            return null;
        }

        return response.StatusCode;
    }

    public async Task<HttpStatusCode?> DeleteAsync(int id)
    {
        using var response = await this.httpService.DeleteAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Tag/{id}"));

        if (response == null)
        {
            return null;
        }

        return response.StatusCode;
    }

    public async Task<IEnumerable<TagDTO>?> GetAllAsync(int id = 0)
    {
        Console.WriteLine($"BaseAddress: {this.httpClient.BaseAddress}");
        //var response = await this.httpClient.GetAsync(new Uri(this.httpClient.BaseAddress!, "/api/Tag"));
        var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, "/api/Tag"));
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
        List<TagDTO>? model = await response.Content.ReadFromJsonAsync<List<TagDTO>>();
        return model;
    }

    public async Task<IEnumerable<TagDTO>?> GetAllByTaskAsync(int id)
    {
        var response = await this.httpService.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Tag?TaskId={id}"));

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
        List<TagDTO>? model = await response.Content.ReadFromJsonAsync<List<TagDTO>>();
        return model;
    }

    public async Task<HttpStatusCode?> UpdateAsync(TagUpdateDTO model, int id)
    {
        ArgumentNullException.ThrowIfNull(model);
        using var todoItemJson = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            Application.Json);

        Console.WriteLine(await todoItemJson.ReadAsStringAsync());
        using var response = await this.httpService.PutAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Tag/{id}"), todoItemJson);

        if (response == null)
        {
            return null;
        }

        return response.StatusCode;
    }

    public Task<TagDTO?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
