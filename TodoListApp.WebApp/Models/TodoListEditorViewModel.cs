using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Models
{
    /// <summary>
    /// Represents the view model for editing a to-do list, containing the list's details.
    /// </summary>
    public class TodoListEditorViewModel : AbstractEditorViewModel
    {
        /// <summary>
        /// Gets or sets the to-do list details.
        /// </summary>
        public TodoListDto List { get; set; } = new TodoListDto();
    }
}
