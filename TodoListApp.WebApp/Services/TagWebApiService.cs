using System.Net;
using System.Text;
using System.Text.Json;
using TodoListApp.WebApi.Helpers.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApp.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace TodoListApp.WebApp.Services;

public class TagWebApiService : ITagWebApiService
{
    private readonly HttpClient httpClient;

    public TagWebApiService(HttpClient client)
    {
        this.httpClient = client;
    }

    public Task<HttpStatusCode?> AddAsync(TagDTO model)
    {
        throw new NotImplementedException();
    }

    public Task<HttpStatusCode?> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TagDTO>?> GetAllAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TagDTO>?> GetAllByTaskAsync(int id)
    {
        var response = await this.httpClient.GetAsync(new Uri(this.httpClient.BaseAddress!, $"/api/Tag?TaskId={id}"));
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


    public Task<HttpStatusCode?> UpdateAsync(TagUpdateDTO model, int id)
    {
        throw new NotImplementedException();
    }

    public Task<TagDTO?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
