using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Models;

public class TaskEditorViewModel : AbstractEditorViewModel
{
    public TaskDTO? Task { get; set; }
}
