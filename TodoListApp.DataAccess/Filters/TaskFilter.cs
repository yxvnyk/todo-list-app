using TodoListApp.DataAccess.Filters.Enums;
using TodoListApp.WebApi.Models.Enums;

namespace TodoListApp.DataAccess.Filters;

/// <summary>
/// Represents the filter criteria for retrieving tasks, inheriting pagination settings.
/// </summary>
public class TaskFilter : PaginationFilter
{
    /// <summary>
    /// Gets or sets the text to search for in the task title.
    /// </summary>
    public string? TextInTitle { get; set; }

    /// <summary>
    /// Gets or sets the status of the task to filter by.
    /// </summary>
    public Status? Status { get; set; }

    /// <summary>
    /// Gets or sets the tag name to filter tasks by.
    /// </summary>
    public string? TagName { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user assigned to the task to filter by.
    /// </summary>
    public string? AssigneeId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who owns the task to filter by.
    /// </summary>
    public string? OwnerId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the to-do list to filter tasks by.
    /// </summary>
    public int TodoListId { get; set; }

    /// <summary>
    /// Gets or sets the overdue status of the task to filter by.
    /// </summary>
    public Overdue? Overdue { get; set; }

    /// <summary>
    /// Gets or sets the field name to sort the tasks by.
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to sort the tasks in descending order.
    /// </summary>
    public bool IsDescending { get; set; }

    /// <summary>
    /// Gets or sets the due date of the task to filter by.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Gets or sets the creation date of the task to filter by.
    /// </summary>
    public DateTime? CreationDate { get; set; }
}
