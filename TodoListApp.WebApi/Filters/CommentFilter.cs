namespace TodoListApp.WebApi.Filters;

public class CommentFilter : PaginationFilter
{
    public int TaskId { get; set; }
}
