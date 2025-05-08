namespace TodoListApp.WebApp.Services.Interfaces
{
    /// <summary>
    /// Defines the basic HTTP operations for making API requests.
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// Sends an HTTP GET request to the specified URL.
        /// </summary>
        /// <param name="url">The URL to which the GET request is sent.</param>
        /// <returns>A task representing the asynchronous operation, containing the HTTP response message.</returns>
        Task<HttpResponseMessage> GetAsync(Uri url);

        /// <summary>
        /// Sends an HTTP PUT request to the specified URL with the provided data.
        /// </summary>
        /// <param name="url">The URL to which the PUT request is sent.</param>
        /// <param name="todoItemJson">The content of the request, typically in JSON format.</param>
        /// <returns>A task representing the asynchronous operation, containing the HTTP response message.</returns>
        Task<HttpResponseMessage> PutAsync(Uri url, StringContent todoItemJson);

        /// <summary>
        /// Sends an HTTP POST request to the specified URL with the provided data.
        /// </summary>
        /// <param name="url">The URL to which the POST request is sent.</param>
        /// <param name="todoItemJson">The content of the request, typically in JSON format.</param>
        /// <returns>A task representing the asynchronous operation, containing the HTTP response message.</returns>
        Task<HttpResponseMessage> PostAsync(Uri url, StringContent todoItemJson);

        /// <summary>
        /// Sends an HTTP DELETE request to the specified URL.
        /// </summary>
        /// <param name="url">The URL to which the DELETE request is sent.</param>
        /// <returns>A task representing the asynchronous operation, containing the HTTP response message.</returns>
        Task<HttpResponseMessage> DeleteAsync(Uri url);
    }
}
