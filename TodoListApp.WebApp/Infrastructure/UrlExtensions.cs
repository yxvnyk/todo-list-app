namespace TodoListApp.WebApp.Infrastructure
{
    /// <summary>
    /// Extension methods for working with URLs in the context of HTTP requests.
    /// </summary>
    public static class UrlExtensions
    {
        /// <summary>
        /// Returns the path and query string from the HTTP request.
        /// </summary>
        /// <param name="request">The current HTTP request.</param>
        /// <returns>
        /// A string containing the path and the query string (if present).
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the provided request is <c>null</c>.
        /// </exception>
        public static string PathAndQuery(this HttpRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            return request.QueryString.HasValue ? $"{request.Path}{request.QueryString}" : request.Path.ToString();
        }
    }
}
