using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.DTO.UpdateDTO
{
    /// <summary>
    /// Represents the data transfer object (DTO) for updating a to-do list.
    /// </summary>
    public class TodoListUpdateDto
    {
        /// <summary>
        /// Gets or sets the title of the to-do list.
        /// </summary>
        /// <remarks>
        /// The title must be a maximum of 50 characters in length.
        /// </remarks>
        [MaxLength(50)]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the to-do list.
        /// </summary>
        /// <remarks>
        /// The description must be a maximum of 200 characters in length.
        /// </remarks>
        [MaxLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with the to-do list.
        /// </summary>
        /// <remarks>
        /// The user ID must be a maximum of 450 characters in length.
        /// </remarks>
        [MaxLength(450)]
        public string? UserId { get; set; }
    }
}
