namespace TodoListApp.WebApp.Models
{
    /// <summary>
    /// Represents the model for error details, including error message, details, and return URL.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Gets or sets the detailed description of the error.
        /// </summary>
        public string? Details { get; set; }

        /// <summary>
        /// Gets or sets the URL to return to after the error.
        /// </summary>
        public Uri? ReturnUrl { get; set; }
    }
}
