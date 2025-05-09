using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.DTO
{
    /// <summary>
    /// Represents a data transfer object (DTO) for user login.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the password for the user.
        /// </summary>
        /// <remarks>
        /// This field is required for authentication.
        /// </remarks>
        [Required]
        public string? Password { get; set; }
    }
}
