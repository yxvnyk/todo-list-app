using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListApp.WebApi.Entities;

[Table("TodoLists")]
public class TodoListEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(450)]
    public string OwnerId { get; set; } = string.Empty;

    public IEnumerable<TaskEntity>? Tasks { get; set; }
}
