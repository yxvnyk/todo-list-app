using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models;

public class TaskModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The Title field is required.")]
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public DateTime? DueDate { get; set; }

    [Required]
    public int TodoListId { get; set; }
}
