using System;
using System.ComponentModel.DataAnnotations;
using TodoListApp.WebApi.Models.CustomValidations;
using TodoListApp.WebApi.Models.Enums;

namespace TodoListApp.WebApi.Models.DTO.UpdateDTO
{
    /// <summary>
    /// Represents the data transfer object (DTO) for updating a task.
    /// </summary>
    public class TaskUpdateDto
    {
        /// <summary>
        /// Gets or sets the title of the task.
        /// </summary>
        /// <remarks>
        /// The title must be a maximum of 50 characters in length.
        /// </remarks>
        [MaxLength(50)]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the task.
        /// </summary>
        /// <remarks>
        /// The description must be a maximum of 200 characters in length.
        /// </remarks>
        [MaxLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the date the task was created.
        /// </summary>
        public DateTime? DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the due date of the task.
        /// </summary>
        /// <remarks>
        /// The due date must be greater than or equal to the current date.
        /// </remarks>
        [MinDateAttribute]
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Gets or sets the status of the task.
        /// </summary>
        /// <remarks>
        /// The status is represented by an enumeration value from <see cref="Status"/>.
        /// </remarks>
        [EnumDataType(typeof(Status), ErrorMessage = "Invalid status value.")]
        public Status? Status { get; set; }

        /// <summary>
        /// Gets or sets the ID of the assignee.
        /// </summary>
        /// <remarks>
        /// The assignee ID must be a maximum of 450 characters in length.
        /// </remarks>
        [MaxLength(450)]
        public string? AssigneeId { get; set; }
    }
}
