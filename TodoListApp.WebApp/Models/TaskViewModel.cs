using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Models;

public class TaskViewModel
{
    public TaskDTO? Task { get; set; } = new();

    public IEnumerable<CommentDTO>? Comments { get; set; }

    public IEnumerable<TagDTO>? Tags { get; set; }

    public string ReturnUrl { get; set; } = "/";
}
