using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models;

public class TagDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The Name field is required.")]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int TaskId { get; set; }
}
