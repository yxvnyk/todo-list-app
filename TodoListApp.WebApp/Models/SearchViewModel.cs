using TodoListApp.WebApi.Models.DTO.PagingDTO;

namespace TodoListApp.WebApp.Models
{
    /// <summary>
    /// Represents the model for searching tasks, including pagination and search filters.
    /// </summary>
    public class SearchViewModel
    {
        /// <summary>
        /// Gets or sets the task pagination information.
        /// </summary>
        public TaskPaging? TaskPagging { get; set; }

        /// <summary>
        /// Gets or sets the search query string used to filter tasks.
        /// </summary>
        public string? Query { get; set; }

        /// <summary>
        /// Gets or sets the field by which tasks are being searched (e.g., title, description).
        /// </summary>
        public string? SearchBy { get; set; }
    }
}
