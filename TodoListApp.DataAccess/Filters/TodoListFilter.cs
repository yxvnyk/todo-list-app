namespace TodoListApp.DataAccess.Filters;

public class TodoListFilter : PaginationFilter
{
    public string? Title { get; set; }

    public string? OwnerId { get; set; }
}
