using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.DTO.UpdateDTO;

public class TodoListUpdateDTO
{
    [MaxLength(50)]
    public string? Title { get; set; }

    [MaxLength(200)]
    public string? Description { get; set; }

    [MaxLength(450)]
    public string? UserId { get; set; }
}
