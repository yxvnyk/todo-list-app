namespace TodoListApp.DataAccess.Filters;

/// <summary>
/// Represents the filter criteria for retrieving to-do lists, inheriting pagination settings.
/// </summary>
public class TodoListFilter : PaginationFilter
{
    /// <summary>
    /// Gets or sets the title of the to-do list to filter by.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who owns the to-do list to filter by.
    /// </summary>
    public string? OwnerId { get; set; }
}
