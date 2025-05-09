using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a tag associated with a task.
    /// </summary>
    public class TagDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the tag.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        /// <remarks>
        /// This field is required and has a maximum length of 50 characters.
        /// </remarks>
        [Required(ErrorMessage = "The Name field is required.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the task associated with the tag.
        /// </summary>
        /// <remarks>
        /// This field is required.
        /// </remarks>
        [Required]
        public int TaskId { get; set; }
    }
}
