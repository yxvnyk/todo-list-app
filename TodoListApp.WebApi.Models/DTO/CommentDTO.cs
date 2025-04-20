using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models;

public class CommentDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The Comment field is required.")]
    [MaxLength(200)]
    public string Comment { get; set; } = string.Empty;

    [Required(ErrorMessage = "The AuthorId field is required.")]
    [MaxLength(200)]
    public string AuthorId { get; set; } = string.Empty;

    public int TaskId { get; set; }
}
