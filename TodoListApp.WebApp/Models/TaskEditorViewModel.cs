using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Models;

public class TaskEditorViewModel
{
    public TaskDTO? Task { get; set; }

    public string? Title { get; set; }

    public string? CallbackMethodName { get; set; }

    public string? ThemeColor { get; set; }
}
