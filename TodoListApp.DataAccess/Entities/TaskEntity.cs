using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoListApp.WebApi.Models.Enums;

namespace TodoListApp.WebApi.Entities;

[Table("Tasks")]
public class TaskEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Description { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DueDate { get; set; }

    public Status Status { get; set; }

    [MaxLength(450)]
    public string AssigneeId { get; set; } = string.Empty;

    [MaxLength(450)]
    public string OwnerId { get; set; } = string.Empty;

    [ForeignKey("TodoList")]
    public int TodoListId { get; set; }

    public TodoListEntity? TodoList { get; set; }

    public IEnumerable<CommentEntity>? Comments { get; set; }

    public IEnumerable<TagEntity>? Tags { get; set; }
}
