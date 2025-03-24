using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListApp.WebApi.Entities;

[Table("Tasks")]
public class TaskEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public DateTime? DueDate { get; set; }

    [ForeignKey("TodoList")]
    public int TodoListId { get; set; }

    public TodoListEntity? TodoList { get; set; }

    public IEnumerable<CommentEntity>? Comments { get; set; }

    public IEnumerable<TagEntity>? Tags { get; set; }
}
