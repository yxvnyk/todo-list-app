namespace TodoListApp.WebApp.Services.Interfaces;

public interface IHttpService
{
    Task<HttpResponseMessage> GetAsync(Uri url);

    Task<HttpResponseMessage> PutAsync(Uri url, StringContent todoItemJson);

    Task<HttpResponseMessage> PostAsync(Uri url, StringContent todoItemJson);

    Task<HttpResponseMessage> DeleteAsync(Uri url);
}
