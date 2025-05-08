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
        public string? ReturnUrl { get; set; }

        // /// <summary>
        // /// Gets or sets the request identifier for debugging purposes.
        // /// </summary>
        // public string? RequestId { get; set; }

        // /// <summary>
        // /// Gets a value indicating whether the request ID should be shown.
        // /// </summary>
        // public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
