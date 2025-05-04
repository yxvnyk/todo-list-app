using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.DTO;
public class LoginDTO
{
    public string? Username { get; set; }

    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}
