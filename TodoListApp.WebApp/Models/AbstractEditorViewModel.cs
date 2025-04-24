using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Models;

public class AbstractEditorViewModel
{
    public string? Title { get; set; }

    public string? CallbackMethodName { get; set; }

    public string? ThemeColor { get; set; }

    public string ReturnUrl { get; set; } = "/";
}
