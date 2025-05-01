using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Models;

public class TaskListPageViewModel
{
    public IEnumerable<TaskDTO>? List { get; set; }

    public string? Title { get; set; }

    public string? ReturnUrl { get; set; }

    public string? AssigneeId { get; set; }

    public int TodoListId { get; set; }
}
