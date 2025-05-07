namespace TodoListApp.DataAccess.Filters;

public class TagFilter : PaginationFilter
{
    public int TaskId { get; set; }

    public int TodoListId { get; set; }

    public string? AssigneeId { get; set; }

    public string? OwnerId { get; set; }
}
