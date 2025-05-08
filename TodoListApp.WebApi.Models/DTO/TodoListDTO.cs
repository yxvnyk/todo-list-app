using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a todo list.
    /// </summary>
    public class TodoListDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the todo list.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the todo list.
        /// </summary>
        /// <remarks>
        /// This field is required and has a maximum length of 50 characters.
        /// </remarks>
        [Required(ErrorMessage = "The Title field is required.")]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the todo list.
        /// </summary>
        /// <remarks>
        /// This field has a maximum length of 200 characters.
        /// </remarks>
        [MaxLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who owns the todo list.
        /// </summary>
        /// <remarks>
        /// This field is required and has a maximum length of 450 characters.
        /// </remarks>
        [Required]
        [MaxLength(450)]
        public string OwnerId { get; set; } = string.Empty;
    }
}
