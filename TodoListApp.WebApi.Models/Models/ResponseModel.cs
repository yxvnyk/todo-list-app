namespace TodoListApp.WebApi.Models.Models
{
    /// <summary>
    /// A generic model representing the response structure for an API request.
    /// </summary>
    /// <typeparam name="T">The type of the data contained in the response.</typeparam>
    public class ResponseModel<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseModel{T}"/> class.
        /// By default, the <see cref="Success"/> property is set to true.
        /// </summary>
        public ResponseModel()
        {
            this.Success = true;
            this.Message = string.Empty;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the request was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the message that provides additional information about the request outcome.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the data returned from the API, which can be of any type.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Gets or sets any errors encountered during the request.
        /// This property is optional and may be null if no errors occurred.
        /// </summary>
        public IEnumerable<string>? Errors { get; set; }
    }
}
