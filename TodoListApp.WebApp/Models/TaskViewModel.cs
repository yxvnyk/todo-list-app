using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Models
{
    /// <summary>
    /// Represents the view model for a task, including task details, comments, tags, owner information, and return URL.
    /// </summary>
    public class TaskViewModel
    {
        /// <summary>
        /// Gets or sets the task details.
        /// </summary>
        public TaskDto? Task { get; set; } = new TaskDto();

        /// <summary>
        /// Gets or sets the collection of comments associated with the task.
        /// </summary>
        public IEnumerable<CommentDto>? Comments { get; set; }

        /// <summary>
        /// Gets or sets the collection of tags associated with the task.
        /// </summary>
        public IEnumerable<TagDto>? Tags { get; set; }

        /// <summary>
        /// Gets or sets the return URL to navigate back after completing the action. Default is "/" (root URL).
        /// </summary>
        public Uri ReturnUrl { get; set; } = default!;

        /// <summary>
        /// Gets or sets the ID of the owner of the task.
        /// </summary>
        public string OwnerId { get; set; } = string.Empty;
    }
}
