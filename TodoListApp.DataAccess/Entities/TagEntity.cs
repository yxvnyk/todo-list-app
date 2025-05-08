using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListApp.WebApi.Entities;

/// <summary>
/// Represents a tag that can be associated with a task in the to-do list.
/// </summary>
[Table("Tags")]
public class TagEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the tag.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the tag.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the task this tag is associated with.
    /// </summary>
    [ForeignKey("Task")]
    public int TaskId { get; set; }

    /// <summary>
    /// Gets or sets the task associated with this tag.
    /// </summary>
    public TaskEntity? Task { get; set; }
}
