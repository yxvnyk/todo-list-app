using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListApp.WebApi.Entities;

/// <summary>
/// Represents a to-do list in the application, which contains tasks.
/// </summary>
[Table("TodoLists")]
public class TodoListEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the to-do list.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the to-do list.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the to-do list.
    /// </summary>
    [MaxLength(200)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who owns the to-do list.
    /// </summary>
    [Required]
    [MaxLength(450)]
    public string OwnerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of tasks associated with the to-do list.
    /// </summary>
    public IEnumerable<TaskEntity>? Tasks { get; set; }
}
