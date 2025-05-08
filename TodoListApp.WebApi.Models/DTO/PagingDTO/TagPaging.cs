namespace TodoListApp.WebApi.Models.DTO.PagingDTO
{
    /// <summary>
    /// Represents a paginated collection of TaskDTO items for tags.
    /// </summary>
    public class TagPaging
    {
        /// <summary>
        /// Gets or sets the collection of TaskDTO items for the current page.
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
