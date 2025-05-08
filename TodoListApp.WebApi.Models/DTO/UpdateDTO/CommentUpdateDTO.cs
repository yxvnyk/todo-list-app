using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.DTO.UpdateDTO
{
    /// <summary>
    /// Represents the data transfer object (DTO) for updating a comment.
    /// </summary>
    public class CommentUpdateDTO
    {
        /// <summary>
        /// Gets or sets the content of the comment.
        /// </summary>
        /// <remarks>
        /// The comment must be a maximum of 200 characters in length.
        /// </remarks>
        [MaxLength(200)]
        public string? Comment { get; set; } = string.Empty;
    }
}
