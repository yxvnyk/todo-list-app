using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListApp.WebApi.Entities;

/// <summary>
/// Represents a comment associated with a task in the to-do list.
/// </summary>
[Table("Comments")]
public class CommentEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the comment.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the textual content of the comment.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string? Comment { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the task this comment is associated with.
    /// </summary>
    [ForeignKey("Task")]
    public int TaskId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the author who created the comment.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string AuthorId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the task associated with this comment.
    /// </summary>
    public TaskEntity? Task { get; set; }
}
