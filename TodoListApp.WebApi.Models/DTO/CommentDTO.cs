using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a comment.
    /// </summary>
    public class CommentDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the comment.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the text content of the comment.
        /// </summary>
        /// <remarks>
        /// The comment text must not exceed 200 characters.
        /// </remarks>
        [Required(ErrorMessage = "The Comment field is required.")]
        [MaxLength(200)]
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the author who wrote the comment.
        /// </summary>
        /// <remarks>
        /// The author ID must not exceed 200 characters.
        /// </remarks>
        [Required(ErrorMessage = "The AuthorId field is required.")]
        [MaxLength(200)]
        public string AuthorId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the task associated with the comment.
        /// </summary>
        public int TaskId { get; set; }
    }
}
