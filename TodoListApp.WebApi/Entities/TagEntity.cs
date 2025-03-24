using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListApp.WebApi.Entities;

[Table("Tags")]
public class TagEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [ForeignKey("Task")]
    public int TaskId { get; set; }

    public TaskEntity? Task { get; set; }
}
