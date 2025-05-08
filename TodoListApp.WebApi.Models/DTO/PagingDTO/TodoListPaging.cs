namespace TodoListApp.WebApi.Models.DTO.PagingDTO
{
    /// <summary>
    /// Represents a paginated collection of TodoListDTO items.
    /// </summary>
    public class TodoListPaging
    {
        /// <summary>
        /// Gets or sets the collection of TodoListDTO items for the current page.
        /// </summary>
        public IEnumerable<TaskDTO>? Items { get; set; }

        /// <summary>
        /// Gets or sets the current page number in the pagination.
        /// </summary>
        public int? CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the total number of items across all pages.
        /// </summary>
        public int TotalCount { get; set; }
    }
}
