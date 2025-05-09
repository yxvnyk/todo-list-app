using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Models
{
    /// <summary>
    /// Represents the view model for editing tasks, extending from the <see cref="AbstractEditorViewModel"/>.
    /// </summary>
    public class TaskEditorViewModel : AbstractEditorViewModel
    {
        /// <summary>
        /// Gets or sets the task data for editing.
        /// </summary>
        public TaskDto Task { get; set; } = new TaskDto();

        /// <summary>
        /// Gets or sets the error message(s) associated with the task editing.
        /// </summary>
        public string? Errors { get; set; }
    }
}
