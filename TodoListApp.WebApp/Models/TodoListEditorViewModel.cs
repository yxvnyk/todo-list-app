using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Models;

public class TodoListEditorViewModel : AbstractEditorViewModel
{
    public TodoListDTO? List { get; set; }
}
