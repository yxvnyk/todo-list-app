using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.DTO
{
    /// <summary>
    /// Represents a data transfer object (DTO) for user registration.
    /// </summary>
    public class RegisterDTO
    {
        /// <summary>
        /// Gets or sets the username for the user registering.
        /// </summary>
        /// <remarks>
        /// This field is required for user registration.
        /// </remarks>
        [Required]
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user registering.
        /// </summary>
        /// <remarks>
        /// This field is required and must be a valid email address.
        /// </remarks>
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the password for the user registering.
        /// </summary>
        /// <remarks>
        /// This field is required for user registration.
        /// </remarks>
        [Required]
        public string? Password { get; set; }
    }
}
