using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoListApp.WebApi.Models.Enums;

namespace TodoListApp.WebApi.Entities;

/// <summary>
/// Represents a task within a to-do list.
/// </summary>
[Table("Tasks")]
public class TaskEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the task.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the task.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    [MaxLength(200)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the date the task was created.
    /// </summary>
    public DateTime DateCreated { get; set; }

    /// <summary>
    /// Gets or sets the due date for the task.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Gets or sets the current status of the task.
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user assigned to the task.
    /// </summary>
    [MaxLength(450)]
    public string AssigneeId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the user who created or owns the task.
    /// </summary>
    [MaxLength(450)]
    public string OwnerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the to-do list this task belongs to.
    /// </summary>
    [ForeignKey("TodoList")]
    public int TodoListId { get; set; }

    /// <summary>
    /// Gets or sets the to-do list that this task is associated with.
    /// </summary>
    public TodoListEntity? TodoList { get; set; }

    /// <summary>
    /// Gets or sets the collection of comments associated with the task.
    /// </summary>
    public IEnumerable<CommentEntity>? Comments { get; set; }

    /// <summary>
    /// Gets or sets the collection of tags associated with the task.
    /// </summary>
    public IEnumerable<TagEntity>? Tags { get; set; }
}
