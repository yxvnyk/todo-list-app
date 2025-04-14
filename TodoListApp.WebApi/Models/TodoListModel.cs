using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models;

public class TodoListModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The Title field is required.")]
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(450)]
    public string UserId { get; set; } = string.Empty;
}
