using System.ComponentModel.DataAnnotations;
using TodoListApp.WebApi.Models.CustomValidations;
using TodoListApp.WebApi.Models.Enums;

namespace TodoListApp.WebApi.Models
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a task.
    /// </summary>
    public class TaskDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the task.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the task.
        /// </summary>
        /// <remarks>
        /// This field is required and has a maximum length of 50 characters.
        /// </remarks>
        [Required(ErrorMessage = "The Title field is required.")]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the task.
        /// </summary>
        /// <remarks>
        /// This field has a maximum length of 200 characters.
        /// </remarks>
        [MaxLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the task was created.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the due date for the task.
        /// </summary>
        /// <remarks>
        /// This field is required and must not be a past date.
        /// </remarks>
        [Required(ErrorMessage = "Due date is required.")]
        [MinDate]
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Gets or sets the status of the task.
        /// </summary>
        /// <remarks>
        /// The status must be one of the predefined values in the <see cref="Status"/> enum.
        /// </remarks>
        [EnumDataType(typeof(Status), ErrorMessage = "Invalid status value.")]
        public Status Status { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user to whom the task is assigned.
        /// </summary>
        /// <remarks>
        /// This field is required and has a maximum length of 450 characters.
        /// </remarks>
        [Required(ErrorMessage = "The AssigneeId field is required.")]
        [MaxLength(450)]
        public string AssigneeId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier of the to-do list to which the task belongs.
        /// </summary>
        /// <remarks>
        /// This field is required.
        /// </remarks>
        [Required]
        public int TodoListId { get; set; }
    }
}
