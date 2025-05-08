using System.Net.Http.Headers;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Services
{
    /// <summary>
    /// Service for sending HTTP requests with authentication headers (JWT token).
    /// </summary>
    public class HttpService : IHttpService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpService"/> class.
        /// </summary>
        /// <param name="httpContext">The <see cref="IHttpContextAccessor"/> used to access the HTTP context, including the session.</param>
        /// <param name="httpClient">The <see cref="HttpClient"/> used for making HTTP requests.</param>
        public HttpService(IHttpContextAccessor httpContext, HttpClient httpClient)
        {
            this.httpContextAccessor = httpContext;
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Sends an HTTP GET request to the specified URL with a JWT token in the authorization header.
        /// </summary>
        /// <param name="url">The URL to send the GET request to.</param>
        /// <returns>A task that represents the asynchronous operation, with an <see cref="HttpResponseMessage"/> as the result.</returns>
        public async Task<HttpResponseMessage> GetAsync(Uri url)
        {
            var token = this.GetJwtToken();

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await this.httpClient.SendAsync(request);
        }

        /// <summary>
        /// Sends an HTTP POST request to the specified URL with a JWT token and content.
        /// </summary>
        /// <param name="url">The URL to send the POST request to.</param>
        /// <param name="todoItemJson">The content to include in the POST request body.</param>
        /// <returns>A task that represents the asynchronous operation, with an <see cref="HttpResponseMessage"/> as the result.</returns>
        public async Task<HttpResponseMessage> PostAsync(Uri url, StringContent todoItemJson)
        {
            var token = this.GetJwtToken();

            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = todoItemJson;
            return await this.httpClient.SendAsync(request);
        }

        /// <summary>
        /// Sends an HTTP DELETE request to the specified URL with a JWT token in the authorization header.
        /// </summary>
        /// <param name="url">The URL to send the DELETE request to.</param>
        /// <returns>A task that represents the asynchronous operation, with an <see cref="HttpResponseMessage"/> as the result.</returns>
        public async Task<HttpResponseMessage> DeleteAsync(Uri url)
        {
            var token = this.GetJwtToken();

            using var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await this.httpClient.SendAsync(request);
        }

        /// <summary>
        /// Sends an HTTP PUT request to the specified URL with a JWT token and content.
        /// </summary>
        /// <param name="url">The URL to send the PUT request to.</param>
        /// <param name="todoItemJson">The content to include in the PUT request body.</param>
        /// <returns>A task that represents the asynchronous operation, with an <see cref="HttpResponseMessage"/> as the result.</returns>
        public async Task<HttpResponseMessage> PutAsync(Uri url, StringContent todoItemJson)
        {
            var token = this.GetJwtToken();

            using var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = todoItemJson;
            return await this.httpClient.SendAsync(request);
        }

        /// <summary>
        /// Retrieves the JWT token from the session.
        /// </summary>
        /// <returns>The JWT token.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the JWT token is not found in the session.</exception>
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
}
