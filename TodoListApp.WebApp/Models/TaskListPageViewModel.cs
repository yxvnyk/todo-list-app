using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Models
{
    /// <summary>
    /// Represents the view model for the task list page, containing tasks and related information for displaying tasks.
    /// </summary>
    public class TaskListPageViewModel
    {
        /// <summary>
        /// Gets or sets the collection of tasks to be displayed on the page.
        /// </summary>
        public IEnumerable<TaskDto>? List { get; set; }

        /// <summary>
        /// Gets or sets the title of the page.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the return URL to navigate back after completing the action.
        /// </summary>
        public Uri? ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets the ID of the assignee (if filtering tasks by assignee).
        /// </summary>
        public string? AssigneeId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the to-do list this task list belongs to.
        /// </summary>
        public int TodoListId { get; set; }
    }
}
