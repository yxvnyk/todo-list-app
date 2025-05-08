namespace TodoListApp.WebApi.Models.Enums;

/// <summary>
/// Represents the different roles a user can have in the application.
/// </summary>
public enum RoleTypes
{
    /// <summary>
    /// A standard user with basic permissions.
    /// </summary>
    User,

    /// <summary>
    /// The owner of the to-do list with full control over the tasks.
    /// </summary>
    Owner,

    /// <summary>
    /// A user with view-only access to the to-do list and tasks.
    /// </summary>
    Viewer,

    /// <summary>
    /// A user with edit permissions for the to-do list and tasks.
    /// </summary>
    Editor,
}
