namespace TodoListApp.WebApp.Models
{
    /// <summary>
    /// Base model for editor views, containing essential properties for interface settings.
    /// </summary>
    public class AbstractEditorViewModel
    {
        /// <summary>
        /// Gets or sets the title displayed in the editor.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets name of the callback method for the editor action.
        /// </summary>
        public string? CallbackMethodName { get; set; }

        /// <summary>
        /// Gets or sets theme color for the editor.
        /// </summary>
        public string? ThemeColor { get; set; }

        /// <summary>
        /// Gets or sets the URL to which the user will be redirected after the action is completed.
        /// Defaults to "/".
        /// </summary>
        public Uri? ReturnUrl { get; set; } = default!;
    }
}
