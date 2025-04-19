using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.DTO.UpdateDTO;

public class CommentUpdateDTO
{
    [MaxLength(200)]
    public string? Comment { get; set; } = string.Empty;
}
