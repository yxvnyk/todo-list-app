using System.Net.Http.Headers;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Services;

public class HttpService : IHttpService
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly HttpClient httpClient;

    public HttpService(IHttpContextAccessor httpContext, HttpClient httpClient)
    {
        this.httpContextAccessor = httpContext;
        this.httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> GetAsync(Uri url)
    {
        var token = this.GetJwtToken();

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await this.httpClient.SendAsync(request);
    }

    public async Task<HttpResponseMessage> PostAsync(Uri url, StringContent todoItemJson)
    {
        var token = this.GetJwtToken();

        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = todoItemJson;
        return await this.httpClient.SendAsync(request);
    }

    public async Task<HttpResponseMessage> DeleteAsync(Uri url)
    {
        var token = this.GetJwtToken();

        using var request = new HttpRequestMessage(HttpMethod.Delete, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await this.httpClient.SendAsync(request);
    }

    public async Task<HttpResponseMessage> PutAsync(Uri url, StringContent todoItemJson)
    {
        var token = this.GetJwtToken();

        using var request = new HttpRequestMessage(HttpMethod.Put, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = todoItemJson;
        return await this.httpClient.SendAsync(request);
    }

    private string GetJwtToken()
    {
        var token = this.httpContextAccessor?.HttpContext?.Session.GetString("JwtToken");
        if (string.IsNullOrEmpty(token))
        {
            throw new UnauthorizedAccessException("JWT token not found in the session.");
        }

        return token;
    }
}
