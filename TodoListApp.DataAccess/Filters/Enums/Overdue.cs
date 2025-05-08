namespace TodoListApp.DataAccess.Filters.Enums;

/// <summary>
/// Represents the status of a task's overdue status.
/// </summary>
public enum Overdue
{
    /// <summary>
    /// Indicates no overdue status (default value).
    /// </summary>
    None = 0,

    /// <summary>
    /// Indicates the task is overdue.
    /// </summary>
    Overdue = 1,

    /// <summary>
    /// Indicates the task is active and not overdue.
    /// </summary>
    Active = 2,
}
