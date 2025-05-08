namespace TodoListApp.WebApi.Models.Enums;

/// <summary>
/// Represents the status of a task.
/// </summary>
public enum Status
{
    /// <summary>
    /// Indicates that the task has not been started yet.
    /// </summary>
    NotStarted = 0,

    /// <summary>
    /// Indicates that the task is currently in progress.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Indicates that the task has been completed.
    /// </summary>
    Completed = 2,
}
