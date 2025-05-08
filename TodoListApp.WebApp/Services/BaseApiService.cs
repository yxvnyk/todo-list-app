using System.Net;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Services
{
    /// <summary>
    /// Provides base functionality for API services that interact with HTTP requests and responses.
    /// </summary>
    public abstract class BaseApiService
    {
        protected readonly HttpClient httpClient;
        protected readonly IHttpService httpService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiService"/> class.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used for making HTTP requests.</param>
        /// <param name="httpService">The service used to handle HTTP requests and responses.</param>
        public BaseApiService(HttpClient httpClient, IHttpService httpService)
        {
            this.httpClient = httpClient;
            this.httpService = httpService;
        }

        /// <summary>
        /// Handles the response from an HTTP request and deserializes the content into a specified type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response content into.</typeparam>
        /// <param name="response">The <see cref="HttpResponseMessage"/> from the HTTP request.</param>
        /// <returns>A task that represents the asynchronous operation, with the deserialized object as the result, or <c>null</c> if the response is not successful.</returns>
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

        /// <summary>
        /// Handles the response status and returns the HTTP status code.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponseMessage"/> from the HTTP request.</param>
        /// <returns>The HTTP status code from the response.</returns>
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

        /// <summary>
        /// Handles the response and returns the content as plain text.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponseMessage"/> from the HTTP request.</param>
        /// <returns>A task that represents the asynchronous operation, with the content as a string, or <c>null</c> if the response is not successful.</returns>
        protected async Task<string?> HandleResponsePlainTextAsync(HttpResponseMessage? response)
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

            return await response.Content.ReadAsStringAsync();
        }
    }
}
