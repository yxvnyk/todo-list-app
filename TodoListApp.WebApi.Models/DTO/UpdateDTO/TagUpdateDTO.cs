using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.DTO.UpdateDTO;

public class TagUpdateDTO
{

    [MaxLength(50)]
    public string? Name { get; set; }
}
