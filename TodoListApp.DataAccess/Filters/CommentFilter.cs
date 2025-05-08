namespace TodoListApp.DataAccess.Filters;

/// <summary>
/// Represents the filter criteria for retrieving comments, inheriting pagination settings.
/// </summary>
public class CommentFilter : PaginationFilter
{
    /// <summary>
    /// Gets or sets the identifier of the task for which to filter comments.
    /// </summary>
    public int TaskId { get; set; }
}
