using System.Net;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Services;

public abstract class BaseApiService
{
    protected readonly HttpClient httpClient;
    protected readonly IHttpService httpService;

    public BaseApiService(HttpClient httpClient, IHttpService httpService)
    {
        this.httpClient = httpClient;
        this.httpService = httpService;
    }

    protected async Task<T?> HandleResponseAsync<T>(HttpResponseMessage? response)
    {
        if (response == null)
        {
            return default;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return default;
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }

        return await response.Content.ReadFromJsonAsync<T>();
    }

    protected HttpStatusCode HandleResponseStatusAsync(HttpResponseMessage? response)
    {
        if (response == null)
        {
            return default;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return default;
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }

        return response.StatusCode;
    }
}
