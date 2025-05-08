namespace TodoListApp.WebApp.Models
{
    /// <summary>
    /// Represents the view model for the complete editor, including title and method details.
    /// </summary>
    public class CompleteEditorViewModel
    {
        /// <summary>
        /// Gets or sets the title displayed in the complete editor.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the method name associated with the complete editor action.
        /// </summary>
        public string? Method { get; set; }
    }
}
