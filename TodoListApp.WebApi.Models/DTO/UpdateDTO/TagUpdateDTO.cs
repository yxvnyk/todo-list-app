using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.DTO.UpdateDTO
{
    /// <summary>
    /// Represents the data transfer object (DTO) for updating a tag.
    /// </summary>
    public class TagUpdateDTO
    {
        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        /// <remarks>
        /// The name of the tag must be a maximum of 50 characters in length.
        /// </remarks>
        [MaxLength(50)]
        public string? Name { get; set; }
    }
}
