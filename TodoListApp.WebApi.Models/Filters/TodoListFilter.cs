namespace TodoListApp.WebApi.Helpers.Filters;

public class TodoListFilter : PaginationFilter
{
    public string? Title { get; set; }

    public string? UserId { get; set; }
}
