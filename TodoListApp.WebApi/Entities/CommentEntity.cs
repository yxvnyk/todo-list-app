using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListApp.WebApi.Entities;

[Table("Comments")]
public class CommentEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string? Comment { get; set; } = string.Empty;

    [ForeignKey("Task")]
    public int TaskId { get; set; }

    public TaskEntity? Task { get; set; }
}
