namespace TodoListApp.DataAccess.Filters;

/// <summary>
/// Represents the filter criteria for retrieving tags, inheriting pagination settings.
/// </summary>
public class TagFilter : PaginationFilter
{
    /// <summary>
    /// Gets or sets the identifier of the task for which to filter tags.
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the to-do list for which to filter tags.
    /// </summary>
    public int TodoListId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user assigned to the task for which to filter tags.
    /// </summary>
    public string? AssigneeId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who owns the task for which to filter tags.
    /// </summary>
    public string? OwnerId { get; set; }
}
