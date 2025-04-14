using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models;

public class CommentModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The Comment field is required.")]
    [MaxLength(200)]
    public string Comment { get; set; } = string.Empty;

    public int TaskId { get; set; }
}
